using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;

namespace toofz.NecroDancer.Dungeons
{
    public sealed class EntityQuadtree : Quadtree<Entity>, ICollection<Entity>
    {
        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public IEnumerable<Entity> Children => GetNodes(Bounds);

        public void Add(Entity item)
        {
            var point = new Point(item.X, item.Y);
            Insert(item, point);
        }

        public bool Contains(Entity item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            foreach (var node in Children)
            {
                array[arrayIndex] = node;
                arrayIndex++;
            }
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            foreach (var node in Children)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override void OnCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    Count++;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Count--;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Count = 0;
                    break;
            }

            base.OnCollectionChanged(action, changedItem);
        }
    }
}
