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
    /// Methods for working with Crypt of the NecroDancer replays.
    /// </summary>
    [RoutePrefix("replays")]
    public sealed class ReplaysController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaysController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        /// <param name="storeClient">The store client used to store submitted data.</param>
        public ReplaysController(
            LeaderboardsContext db,
            ILeaderboardsStoreClient storeClient)
        {
            this.db = db;
            this.storeClient = storeClient;
        }

        readonly LeaderboardsContext db;
        readonly ILeaderboardsStoreClient storeClient;

        [ResponseType(typeof(Models.Replays))]
        [Route("")]
        public async Task<IHttpActionResult> GetReplays(
            int? version = null,
            int? error = null,
            [FromUri] ReplaysPagination pagination = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            pagination = pagination ?? new ReplaysPagination();

            var query = from r in db.Replays
                        where r.Version == version && r.ErrorCode == error
                        orderby r.ReplayId
                        select new Models.Replay
                        {
                            id = r.ReplayId.ToString(),
                            error = r.ErrorCode,
                            seed = r.Seed,
                            version = r.Version,
                            killed_by = r.KilledBy,
                        };

            var total = await query.CountAsync(cancellationToken);
            var replays = await query
                .Skip(pagination.offset)
                .Take(pagination.limit)
                .ToListAsync(cancellationToken);

            var results = new Models.Replays
            {
                total = total,
                replays = replays,
            };

            return Ok(results);
        }

        /// <summary>
        /// Updates replays.
        /// </summary>
        /// <param name="replays">A list of replays.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns the number of replays updated.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// Any replays failed validation.
        /// </httpStatusCode>
        [ResponseType(typeof(BulkStoreDTO))]
        [Route("")]
        [Authorize(Users = "ReplaysService")]
        public async Task<IHttpActionResult> PostReplays(
            IEnumerable<ReplayModel> replays,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = (from r in replays
                         select new Leaderboards.Replay
                         {
                             ReplayId = r.ReplayId,
                             ErrorCode = r.ErrorCode,
                             Seed = r.Seed,
                             Version = r.Version,
                             KilledBy = r.KilledBy,
                         }).ToList();

            await storeClient.SaveChangesAsync(model, true, cancellationToken);

            return Ok(new BulkStoreDTO { RowsAffected = replays.Count() });
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
