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
        /// <param name="sqlClient">The SQL client used to store submitted data.</param>
        public ReplaysController(
            LeaderboardsContext db,
            ILeaderboardsSqlClient sqlClient)
        {
            this.db = db;
            this.sqlClient = sqlClient;
        }

        private readonly LeaderboardsContext db;
        private readonly ILeaderboardsSqlClient sqlClient;

        /// <summary>
        /// Gets a list of UGCIDs that require processing.
        /// </summary>
        /// <param name="limit">The maximum number of results to return.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns UGCIDs that require processing.
        /// </returns>
        [ResponseType(typeof(List<long>))]
        [Route("")]
        [Authorize(Users = "ReplaysService")]
        public async Task<IHttpActionResult> Get(int limit,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var missing = await (from r in db.Replays
                                 where r.Version == null && r.ErrorCode == null
                                 select r.ReplayId)
                                 .Take(limit)
                                 .ToListAsync(cancellationToken);

            return Ok(missing);
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
        public async Task<IHttpActionResult> Post(IEnumerable<ReplayModel> replays,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = (from r in replays
                         select new Replay
                         {
                             ReplayId = r.ReplayId,
                             ErrorCode = r.ErrorCode,
                             Seed = r.Seed,
                             Version = r.Version,
                             KilledBy = r.KilledBy,
                         }).ToList();

            await sqlClient.SaveChangesAsync(model, true, cancellationToken);

            return Ok(new BulkStoreDTO { RowsAffected = replays.Count() });
        }

        #region IDisposable Members

        private bool disposed;

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
