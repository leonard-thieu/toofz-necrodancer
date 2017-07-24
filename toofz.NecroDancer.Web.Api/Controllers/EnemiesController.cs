using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.EntityFramework;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    /// <summary>
    /// Methods for working with Crypt of the NecroDancer enemies.
    /// </summary>
    [RoutePrefix("enemies")]
    public sealed class EnemiesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemiesController"/> class.
        /// </summary>
        /// <param name="db">The NecroDancer context.</param>
        public EnemiesController(NecroDancerContext db)
        {
            this.db = db;
        }

        private readonly NecroDancerContext db;

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer enemies.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer enemies.
        /// </returns>
        [ResponseType(typeof(Enemies))]
        [Route("")]
        public async Task<IHttpActionResult> Get(
            [FromUri] EnemiesPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var content = await GetEnemiesAsync(OptionalStats.None, pagination, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer enemies with a specific attribute.
        /// </summary>
        /// <param name="attribute">The enemy's attribute.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer enemies with the attribute.
        /// </returns>
        [ResponseType(typeof(Enemies))]
        [Route("{attribute}")]
        public async Task<IHttpActionResult> Get(OptionalStats attribute,
            [FromUri] EnemiesPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var content = await GetEnemiesAsync(attribute, pagination, cancellationToken);

            return Ok(content);
        }

        internal async Task<Enemies> GetEnemiesAsync(OptionalStats attribute,
            EnemiesPagination pagination,
            CancellationToken cancellationToken)
        {
            var p = pagination ?? new EnemiesPagination();
            var offset = p.offset;
            var limit = p.limit;

            var query = from e in db.Enemies
                        where (e.OptionalStats & attribute) == attribute
                        orderby e.ElementName, e.Type
                        select new Models.Enemy
                        {
                            name = e.ElementName,
                            type = e.Type,
                            display_name = e.Name,
                            health = e.Stats.Health,
                            damage = e.Stats.DamagePerHit,
                            beats_per_move = e.Stats.BeatsPerMove,
                            drops = e.Stats.CoinsToDrop
                        };

            var total = await query.CountAsync(cancellationToken);

            var enemies = await query
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return new Enemies
            {
                total = total,
                enemies = enemies
            };
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
