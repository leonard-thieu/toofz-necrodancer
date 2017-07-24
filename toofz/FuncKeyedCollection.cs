using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace toofz
{
    public class FuncKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
    {
        public FuncKeyedCollection(Func<TItem, TKey> getKeyForItem)
            : base()
        {
            if (getKeyForItem == null)
                throw new ArgumentNullException(nameof(getKeyForItem));

            this.getKeyForItem = getKeyForItem;
        }

        private readonly Func<TItem, TKey> getKeyForItem;

        public virtual void AddRange(IEnumerable<TItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                var key = GetKeyForItem(item);
                if (!Contains(key))
                {
                    Add(item);
                }
            }
        }

        public bool TryGetValue(TKey key, out TItem value)
        {
            if (Contains(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = default(TItem);
                return false;
            }
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return getKeyForItem(item);
        }
    }
}
