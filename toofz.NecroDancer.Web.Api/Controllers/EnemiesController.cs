using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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

        readonly NecroDancerContext db;

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
        public async Task<IHttpActionResult> GetEnemies(
            [FromUri] EnemiesPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var content = await GetEnemiesAsync(null, pagination, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer enemies with a specific attribute.
        /// </summary>
        /// <param name="attribute">
        /// The enemy's attribute.
        /// Valid values are 'boss', 'bounce-on-movement-fail', 'floating', 'ignore-liquids', 'ignore-walls', 'is-monkey-like', 'massive', and 'miniboss'.
        /// </param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer enemies with the attribute.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// Enemy attribute is invalid.
        /// </httpStatusCode>
        [ResponseType(typeof(Enemies))]
        [Route("{attribute}")]
        public async Task<IHttpActionResult> GetEnemies(
            string attribute,
            [FromUri] EnemiesPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var content = await GetEnemiesAsync(attribute, pagination, cancellationToken);

                return Ok(content);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        internal async Task<Enemies> GetEnemiesAsync(
            string attribute,
            EnemiesPagination pagination,
            CancellationToken cancellationToken)
        {
            var p = pagination ?? new EnemiesPagination();
            var offset = p.offset;
            var limit = p.limit;

            var baseQuery = from e in db.Enemies
                            select e;
            if (attribute != null)
            {
                switch (attribute)
                {
                    case "boss": baseQuery = baseQuery.Where(e => e.OptionalStats.Boss); break;
                    case "bounce-on-movement-fail": baseQuery = baseQuery.Where(e => e.OptionalStats.BounceOnMovementFail); break;
                    case "floating": baseQuery = baseQuery.Where(e => e.OptionalStats.Floating); break;
                    case "ignore-liquids": baseQuery = baseQuery.Where(e => e.OptionalStats.IgnoreLiquids); break;
                    case "ignore-walls": baseQuery = baseQuery.Where(e => e.OptionalStats.IgnoreWalls); break;
                    case "is-monkey-like": baseQuery = baseQuery.Where(e => e.OptionalStats.IsMonkeyLike); break;
                    case "massive": baseQuery = baseQuery.Where(e => e.OptionalStats.Massive); break;
                    case "miniboss": baseQuery = baseQuery.Where(e => e.OptionalStats.Miniboss); break;
                    default: throw new ArgumentException("Enemy attribute is invalid.");
                }
            }
            var query = from e in baseQuery
                        orderby e.ElementName, e.Type
                        select new Models.Enemy
                        {
                            name = e.ElementName,
                            type = e.Type,
                            display_name = e.Name,
                            health = e.Stats.Health,
                            damage = e.Stats.DamagePerHit,
                            beats_per_move = e.Stats.BeatsPerMove,
                            drops = e.Stats.CoinsToDrop,
                        };

            var total = await query.CountAsync(cancellationToken);

            var enemies = await query
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return new Enemies
            {
                total = total,
                enemies = enemies,
            };
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
