using System;
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
    /// Methods for working with Crypt of the NecroDancer leaderboards.
    /// </summary>
    [RoutePrefix("leaderboards")]
    public sealed class LeaderboardsController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderboardsController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        /// <param name="categories">Leaderboard categories.</param>
        /// <param name="leaderboardHeaders">Leaderboard headers.</param>
        public LeaderboardsController(
            LeaderboardsContext db,
            Categories categories,
            LeaderboardHeaders leaderboardHeaders)
        {
            this.db = db;
            this.categories = categories;
            this.leaderboardHeaders = leaderboardHeaders;
        }

        readonly LeaderboardsContext db;
        readonly Categories categories;
        readonly LeaderboardHeaders leaderboardHeaders;

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer leaderboards.
        /// </summary>
        /// <param name="products">
        /// Valid values are 'classic' and 'amplified'. 
        /// If not provided, returns leaderboards from all products.
        /// </param>
        /// <param name="modes">
        /// Valid values are 'standard', 'no-return', 'hard', 'phasing', 'randomzier', and 'mystery'. 
        /// If not provided, returns leaderboards from all modes.
        /// </param>
        /// <param name="runs">
        /// Valid values are 'score', 'speed', 'seeded-score', 'seeded-speed', and 'deathless'. 
        /// If not provided, returns leaderboards from all runs.
        /// </param>
        /// <param name="characters">
        /// Valid values are 'all-characters', 'all-characters-amplified', 'aria', 'bard', 'bolt', 'cadence', 'coda', 'diamond', 'dorian', 'dove', 'eli', 'mary', 'melody', 'monk', 'nocturna', 'story-mode', and 'tempo'. 
        /// If not provided, returns leaderboards from all characters.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer leaderboards.
        /// </returns>
        [ResponseType(typeof(Models.Leaderboards))]
        [Route("")]
        public async Task<IHttpActionResult> GetLeaderboards(
            string products = null, string modes = null, string runs = null, string characters = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            products = products ?? categories.GetAllItemNames("products");
            modes = modes ?? categories.GetAllItemNames("modes");
            runs = runs ?? categories.GetAllItemNames("runs");
            characters = characters ?? categories.GetAllItemNames("characters");

            var product_names = products.Split(',');
            var mode_names = modes.Split(',');
            var run_names = runs.Split(',');
            var character_names = characters.Split(',');

            var lbids = (from h in leaderboardHeaders
                         where product_names.Contains(h.product)
                         where mode_names.Contains(h.mode)
                         where run_names.Contains(h.run)
                         where character_names.Contains(h.character)
                         select h.id).ToList();

            var query = await (from l in db.Leaderboards
                               where lbids.Contains(l.LeaderboardId)
                               select new
                               {
                                   l.LeaderboardId,
                                   l.CharacterId,
                                   l.RunId,
                                   l.LastUpdate,
                                   l.Entries.Count
                               }).ToListAsync(cancellationToken);

            var leaderboards = (from l in query
                                join h in leaderboardHeaders on l.LeaderboardId equals h.id
                                orderby h.product, h.character, l.RunId
                                select new Models.Leaderboard
                                {
                                    id = h.id,
                                    product = h.product,
                                    character = h.character,
                                    mode = h.mode,
                                    run = h.run,
                                    display_name = h.display_name,
                                    updated_at = l.LastUpdate,
                                    total = l.Count
                                }).ToList();

            var vm = new Models.Leaderboards
            {
                total = leaderboards.Count,
                leaderboards = leaderboards
            };

            return Ok(vm);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer leaderboard entries.
        /// </summary>
        /// <param name="lbid">The leaderboard ID.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a list of Crypt of the NecroDancer leaderboard entries.</returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// The leaderboard does not exist.
        /// </httpStatusCode>
        [ResponseType(typeof(LeaderboardEntries))]
        [Route("{lbid:int}/entries")]
        public async Task<IHttpActionResult> GetLeaderboardEntries(int lbid,
            [FromUri] LeaderboardsPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            pagination = pagination ?? new LeaderboardsPagination();

            var leaderboard = await db.Leaderboards.FirstOrDefaultAsync(l => l.LeaderboardId == lbid, cancellationToken);
            if (leaderboard == null)
            {
                return NotFound();
            }

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

            var total = await query.CountAsync(cancellationToken);

            var entriesPage = await query
                .Skip(pagination.offset)
                .Take(pagination.limit)
                .ToListAsync(cancellationToken);

            var replayIds = entriesPage.Select(entry => entry.ReplayId);
            var replays = await (from r in db.Replays
                                 where replayIds.Contains(r.ReplayId)
                                 select new
                                 {
                                     r.ReplayId,
                                     r.KilledBy,
                                     r.Version,
                                 }).ToListAsync(cancellationToken);

            var entries = (from e in entriesPage
                           join r in replays on e.ReplayId equals r.ReplayId into g
                           from x in g.DefaultIfEmpty()
                           select new Models.Entry
                           {
                               player = new Models.Player
                               {
                                   id = e.Player.SteamId.ToString(),
                                   display_name = e.Player.Name,
                                   updated_at = e.Player.LastUpdate,
                                   avatar = e.Player.Avatar,
                               },
                               rank = e.Rank,
                               score = e.Score,
                               end = new End
                               {
                                   zone = e.End.Zone,
                                   level = e.End.Level
                               },
                               killed_by = x?.KilledBy,
                               version = x?.Version,
                           }).ToList();

            var h = leaderboardHeaders.FirstOrDefault(l => l.id == leaderboard.LeaderboardId);
            if (h == null)
            {
                return NotFound();
            }

            var content = new LeaderboardEntries
            {
                leaderboard = new Models.Leaderboard
                {
                    id = h.id,
                    product = h.product,
                    character = h.character,
                    mode = h.mode,
                    run = h.run,
                    display_name = h.display_name,
                    updated_at = leaderboard.LastUpdate,
                },
                total = total,
                entries = entries,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer daily leaderboards.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="products">
        /// Valid values are 'classic' and 'amplified'. 
        /// If not provided, returns daily leaderboards from all products.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer daily leaderboards.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// A product is invalid.
        /// </httpStatusCode>
        [ResponseType(typeof(DailyLeaderboards))]
        [Route("dailies")]
        public async Task<IHttpActionResult> GetDailies(
            [FromUri] LeaderboardsPagination pagination,
            string products = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            pagination = pagination ?? new LeaderboardsPagination();
            products = products ?? categories.GetAllItemNames("products");

            var productIds = new List<int>();
            foreach (var p in products.Split(','))
            {
                try
                {
                    productIds.Add(categories.GetItemId("products", p));
                }
                // TODO: When GetId is changed to throw a more specific Exception, this should also be updated.
                catch (Exception)
                {
                    return BadRequest($"'{p}' is not a valid product.");
                }
            }

            var query = await (from l in db.DailyLeaderboards
                               where productIds.Contains(l.ProductId)
                               orderby l.Date descending, l.ProductId
                               select new
                               {
                                   l.LeaderboardId,
                                   l.Date,
                                   l.LastUpdate,
                                   l.ProductId,
                                   l.IsProduction,
                               })
                               .Skip(pagination.offset)
                               .Take(pagination.limit)
                               .ToListAsync(cancellationToken);

            var leaderboards = (from l in query
                                select new Models.DailyLeaderboard
                                {
                                    id = l.LeaderboardId,
                                    date = l.Date,
                                    updated_at = l.LastUpdate,
                                    product = categories.GetItemName("products", l.ProductId),
                                    production = l.IsProduction,
                                })
                                .ToList();

            var vm = new DailyLeaderboards
            {
                total = leaderboards.Count,
                leaderboards = leaderboards,
            };

            return Ok(vm);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer daily leaderboard entries.
        /// </summary>
        /// <param name="lbid">The daily leaderboard ID.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a list of Crypt of the NecroDancer daily leaderboard entries.</returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// The daily leaderboard does not exist.
        /// </httpStatusCode>
        [ResponseType(typeof(DailyLeaderboardEntries))]
        [Route("dailies/{lbid:int}/entries")]
        public async Task<IHttpActionResult> GetDailyLeaderboardEntries(int lbid,
            [FromUri] LeaderboardsPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var p = pagination ?? new LeaderboardsPagination();

            var leaderboard = await db.DailyLeaderboards.FirstOrDefaultAsync(l => l.LeaderboardId == lbid, cancellationToken);
            if (leaderboard == null)
            {
                return NotFound();
            }

            var query = from e in db.DailyEntries
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

            var total = await query.CountAsync(cancellationToken);

            var entriesPage = await query
                .Skip(p.offset)
                .Take(p.limit)
                .ToListAsync(cancellationToken);

            var replayIds = entriesPage.Select(entry => entry.ReplayId);
            var replays = await (from r in db.Replays
                                 where replayIds.Contains(r.ReplayId)
                                 select new
                                 {
                                     r.ReplayId,
                                     r.KilledBy,
                                     r.Version,
                                 }).ToListAsync(cancellationToken);

            var entries = (from e in entriesPage
                           join r in replays on e.ReplayId equals r.ReplayId into g
                           from x in g.DefaultIfEmpty()
                           select new Models.Entry
                           {
                               player = new Models.Player
                               {
                                   id = e.Player.SteamId.ToString(),
                                   display_name = e.Player.Name,
                                   updated_at = e.Player.LastUpdate,
                                   avatar = e.Player.Avatar,
                               },
                               rank = e.Rank,
                               score = e.Score,
                               end = new End
                               {
                                   zone = e.End.Zone,
                                   level = e.End.Level
                               },
                               killed_by = x?.KilledBy,
                               version = x?.Version,
                           }).ToList();

            var content = new DailyLeaderboardEntries
            {
                leaderboard = new Models.DailyLeaderboard
                {
                    id = leaderboard.LeaderboardId,
                    date = leaderboard.Date,
                    updated_at = leaderboard.LastUpdate,
                    product = categories.GetItemName("products", leaderboard.ProductId),
                    production = leaderboard.IsProduction,
                },
                total = total,
                entries = entries
            };

            return Ok(content);
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
