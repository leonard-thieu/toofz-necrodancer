using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using toofz.NecroDancer.EntityFramework;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    /// <summary>
    /// Methods for working with Crypt of the NecroDancer items.
    /// </summary>
    [RoutePrefix("items")]
    public sealed class ItemsController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="db">The NecroDancer context.</param>
        public ItemsController(NecroDancerContext db)
        {
            this.db = db;
        }

        private readonly NecroDancerContext db;

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer items.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer items.
        /// </returns>
        [ResponseType(typeof(Items))]
        [Route("")]
        public async Task<IHttpActionResult> Get(
            [FromUri] ItemsPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var content = await GetItemsAsync(i => true, pagination, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer items in a specific category.
        /// </summary>
        /// <param name="category">The category of items to return.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer items in the category.
        /// </returns>
        [ResponseType(typeof(Items))]
        [Route("{category}")]
        public async Task<IHttpActionResult> Get(string category,
            [FromUri] ItemsPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var filter = Items(category, null);
            var content = await GetItemsAsync(filter, pagination, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer items in a specific subcategory.
        /// </summary>
        /// <param name="category">The category of items to return.</param>
        /// <param name="subcategory">The subcategory within the category.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer items in the subcategory.
        /// </returns>
        [ResponseType(typeof(Items))]
        [Route("{category}/{subcategory}")]
        public async Task<IHttpActionResult> Get(string category, string subcategory,
            [FromUri] ItemsPagination pagination,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var filter = Items(category, subcategory);
            var content = await GetItemsAsync(filter, pagination, cancellationToken);

            return Ok(content);
        }

        private async Task<Items> GetItemsAsync(Expression<Func<Data.Item, bool>> filter, ItemsPagination pagination, CancellationToken cancellationToken)
        {
            var p = pagination ?? new ItemsPagination();
            var offset = p.offset;
            var limit = p.limit;

            var query = db.Items
                .Where(filter)
                .OrderBy(i => i.ElementName)
                .Select(i => new Item
                {
                    name = i.ElementName,
                    display_name = i.Name,
                    slot = i.Slot,
                    unlock = i.DiamondCost,
                    cost = i.CoinCost,
                });

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return new Items
            {
                total = total,
                items = items
            };
        }

        private static Expression<Func<Data.Item, bool>> Items(string category, string subcategory)
        {
            category = category?.ToLowerInvariant();
            subcategory = subcategory?.ToLowerInvariant();

            switch (category)
            {
                case "armor": return i => i.IsArmor;
                case "consumable": return i => i.Consumable;
                case "feet": return i => i.Slot == "feet";
                case "food": return i => i.IsFood;
                case "head": return i => i.Slot == "head";
                case "rings": return i => i.Slot == "ring";
                case "scrolls": return i => i.IsScroll;
                case "spells": return i => i.IsSpell;
                case "torches": return i => i.IsTorch;
                case "weapons": return GetWeaponFilter(subcategory);
                case "chest": return GetChestFilter(subcategory);

                default:
                    throw new ArgumentException("Invalid item category.", nameof(category));
            }
        }

        private static Expression<Func<Data.Item, bool>> GetWeaponFilter(string subcategory)
        {
            switch (subcategory)
            {
                case "bows": return i => i.IsBow;
                case "broadswords": return i => i.IsBroadsword;
                case "cats": return i => i.IsCat;
                case "crossbows": return i => i.IsCrossbow;
                case "daggers": return i => i.IsDagger;
                case "flails": return i => i.IsFlail;
                case "longswords": return i => i.IsLongsword;
                case "rapiers": return i => i.IsRapier;
                case "spears": return i => i.IsSpear;
                case "whips": return i => i.IsWhip;
                case null: return i => i.IsWeapon;

                default:
                    throw new ArgumentException("Invalid weapon category.", nameof(subcategory));
            }
        }

        private static readonly string[] RedChestSlots = new[] { "head", "hud", "hud_weapon", "action", "bomb", "shovel" };
        private static readonly string[] PurpleChestSlots = new[] { "ring" };
        private static readonly string[] BlackChestSlots = new[] { "feet" };

        private static Expression<Func<Data.Item, bool>> GetChestFilter(string subcategory)
        {
            switch (subcategory)
            {
                case "red": return i => (i.IsFood || i.IsTorch || i.IsShovel || RedChestSlots.Contains(i.Slot)) && !i.IsScroll;
                case "purple": return i => i.IsSpell || i.IsScroll || PurpleChestSlots.Contains(i.Slot);
                case "black": return i => i.IsArmor || i.IsWeapon || BlackChestSlots.Contains(i.Slot);
                case "mimic": return i => true;

                default:
                    throw new ArgumentException("Invalid chest category.", nameof(subcategory));
            }
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
