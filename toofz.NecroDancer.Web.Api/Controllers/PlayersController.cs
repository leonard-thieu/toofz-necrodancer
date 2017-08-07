using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    /// <summary>
    /// Methods for working with Steam players.
    /// </summary>
    [RoutePrefix("players")]
    public sealed class PlayersController : ApiController
    {
        static IReadOnlyDictionary<string, string> SortKeySelectorMap = new Dictionary<string, string>
        {
            { $"{nameof(Models.Player.id)}", $"{nameof(Leaderboards.Player.SteamId)}" },
            { $"{nameof(Models.Player.display_name)}", $"{nameof(Leaderboards.Player.Name)}" },
            { $"{nameof(Models.Player.updated_at)}", $"{nameof(Leaderboards.Player.LastUpdate)}" },
            { $"{nameof(Models.PlayerEntries.entries)}", $"{nameof(Leaderboards.Player.Entries)}.{nameof(List<Leaderboards.Entry>.Count)}" },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        /// <param name="storeClient">The leaderboards store client.</param>
        /// <param name="leaderboardHeaders">Leaderboard headers.</param>
        public PlayersController(
            LeaderboardsContext db,
            ILeaderboardsStoreClient storeClient,
            LeaderboardHeaders leaderboardHeaders)
        {
            this.db = db;
            this.storeClient = storeClient;
            this.leaderboardHeaders = leaderboardHeaders;
        }

        readonly LeaderboardsContext db;
        readonly ILeaderboardsStoreClient storeClient;
        readonly LeaderboardHeaders leaderboardHeaders;

        /// <summary>
        /// Search for Steam players.
        /// </summary>
        /// <param name="q">A search query.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="sort">
        /// Comma-separated values of properties to sort by. Properties may be prefixed with "-" to sort descending.
        /// Valid properties are "id", "display_name", "updated_at", and "entries".
        /// If not provided, results will sorted using "-entries,display_name,id".
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Steam players that match the search query.
        /// </returns>
        [ResponseType(typeof(Players))]
        [Route("")]
        public async Task<IHttpActionResult> GetPlayers(
            string q = null,
            [FromUri] PlayersPagination pagination = null,
            string sort = "-entries,display_name,id",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            pagination = pagination ?? new PlayersPagination();

            IQueryable<Leaderboards.Player> queryBase = db.Players;
            if (!string.IsNullOrEmpty(q))
            {
                queryBase = queryBase.Where(p => p.Name.StartsWith(q));
            }

            if (queryBase.TryApplySort(sort, SortKeySelectorMap, out IQueryable<Leaderboards.Player> sorted))
            {
                queryBase = sorted;
            }

            var query = from p in queryBase
                        select new Models.Player
                        {
                            id = p.SteamId.ToString(),
                            display_name = p.Name,
                            updated_at = p.LastUpdate,
                            avatar = p.Avatar,
                        };

            var total = await query.CountAsync(cancellationToken);
            var players = await query
                .Skip(pagination.offset)
                .Take(pagination.limit)
                .ToListAsync(cancellationToken);

            var results = new Players
            {
                total = total,
                players = players,
            };

            return Ok(results);
        }

        /// <summary>
        /// Gets a Steam player's leaderboard entries.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a Steam player's leaderboard entries.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(PlayerEntries))]
        [Route("{steamId}/entries")]
        public async Task<IHttpActionResult> GetPlayer(
            long steamId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var player = db.Players.FirstOrDefault(p => p.SteamId == steamId);
            if (player == null)
            {
                return NotFound();
            }

            var playerEntries = await (from e in db.Entries
                                       where e.SteamId == steamId && e.Leaderboard.Date == null
                                       select new
                                       {
                                           Leaderboard = new
                                           {
                                               e.Leaderboard.LeaderboardId,
                                               e.Leaderboard.CharacterId,
                                               e.Leaderboard.RunId,
                                               e.Leaderboard.LastUpdate,
                                           },
                                           Rank = e.Rank,
                                           Score = e.Score,
                                           End = new
                                           {
                                               e.Zone,
                                               e.Level,
                                           },
                                           ReplayId = e.ReplayId,
                                       }).ToListAsync(cancellationToken);

            var replayIds = playerEntries.Select(entry => entry.ReplayId);
            var replays = await (from r in db.Replays
                                 where replayIds.Contains(r.ReplayId)
                                 select new
                                 {
                                     r.ReplayId,
                                     r.KilledBy,
                                     r.Version,
                                 }).ToListAsync(cancellationToken);

            var entries = (from e in playerEntries
                           join r in replays on e.ReplayId equals r.ReplayId into g
                           from x in g.DefaultIfEmpty()
                           join h in leaderboardHeaders on e.Leaderboard.LeaderboardId equals h.id
                           orderby h.product, e.Leaderboard.RunId, h.character
                           select new Models.Entry
                           {
                               leaderboard = new Models.Leaderboard
                               {
                                   id = h.id,
                                   product = h.product,
                                   character = h.character,
                                   mode = h.mode,
                                   run = h.run,
                                   updated_at = e.Leaderboard.LastUpdate,
                               },
                               rank = e.Rank,
                               score = e.Score,
                               end = new End
                               {
                                   zone = e.End.Zone,
                                   level = e.End.Level,
                               },
                               killed_by = x?.KilledBy,
                               version = x?.Version,
                           }).ToList();

            var vm = new PlayerEntries
            {
                player = new Models.Player
                {
                    id = player.SteamId.ToString(),
                    display_name = player.Name,
                    updated_at = player.LastUpdate,
                    avatar = player.Avatar,
                },
                total = entries.Count,
                entries = entries,
            };

            return Ok(vm);
        }

        /// <summary>
        /// Gets a player's leaderboard entry for a leaderboard.
        /// </summary>
        /// <param name="lbid">The ID of the leaderboard.</param>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a player's leaderboard entry for a leaderboard.</returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// An entry for the player on the leaderboard was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(Models.Entry))]
        [Route("{steamId}/entries/{lbid:int}")]
        public async Task<IHttpActionResult> GetPlayerLeaderboardEntry(
            int lbid,
            long steamId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from e in db.Entries
                        where e.LeaderboardId == lbid
                        orderby e.Rank
                        select new
                        {
                            Player = new
                            {
                                e.Player.SteamId,
                                e.Player.Name,
                                e.Player.LastUpdate,
                                e.Player.Avatar,
                            },
                            Rank = e.Rank,
                            Score = e.Score,
                            End = new
                            {
                                e.Zone,
                                e.Level,
                            },
                            ReplayId = e.ReplayId,
                        };

            var playerEntry = await query.FirstOrDefaultAsync(e => e.Player.SteamId == steamId, cancellationToken);
            if (playerEntry == null)
            {
                return NotFound();
            }

            var replay = await (from r in db.Replays
                                where r.ReplayId == playerEntry.ReplayId
                                select new
                                {
                                    r.KilledBy,
                                    r.Version,
                                })
                                .FirstOrDefaultAsync(cancellationToken);

            var entry = new Models.Entry
            {
                player = new Models.Player
                {
                    id = playerEntry.Player.SteamId.ToString(),
                    display_name = playerEntry.Player.Name,
                    updated_at = playerEntry.Player.LastUpdate,
                    avatar = playerEntry.Player.Avatar,
                },
                rank = playerEntry.Rank,
                score = playerEntry.Score,
                end = new End
                {
                    zone = playerEntry.End.Zone,
                    level = playerEntry.End.Level,
                },
                killed_by = replay?.KilledBy,
                version = replay?.Version,
            };

            return Ok(entry);
        }

        /// <summary>
        /// Updates Steam players.
        /// </summary>
        /// <param name="players">A list of players.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns the number of Steam players updated.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// Any players failed validation.
        /// </httpStatusCode>
        [ResponseType(typeof(BulkStore))]
        [Route("")]
        [Authorize(Users = "PlayersService")]
        public async Task<IHttpActionResult> PostPlayers(
            IEnumerable<PlayerModel> players,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = (from p in players
                         select new Leaderboards.Player
                         {
                             SteamId = p.SteamId.Value,
                             Exists = p.Exists,
                             Name = p.Name,
                             LastUpdate = p.LastUpdate,
                             Avatar = p.Avatar,
                         }).ToList();
            await storeClient.SaveChangesAsync(model, true, cancellationToken);

            return Ok(new BulkStore { rows_affected = players.Count() });
        }

        #region IDisposable Members

        bool disposed;

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                db.Dispose();
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
