using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;

namespace toofz.NecroDancer.Dungeons
{
    public partial class Quadtree<T> : INotifyCollectionChanged
        where T : class
    {
        Quadrant root;
        readonly IDictionary<T, Quadrant> table = new Dictionary<T, Quadrant>();

        /// <summary>
        /// The bounds of this tree. Changing this value will cause the tree to be re-indexed.
        /// </summary>
        public RectangleF Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; ReIndex(); }
        }
        RectangleF _Bounds;

        void ReIndex()
        {
            // Suppress collection change notifications during re-indexing since the collection is not 
            // actually changing, just its internal structure.
            IsCollectionChangedEnabled = false;

            var nodes = GetQuadNodes(Bounds);
            Clear();
            foreach (var n in nodes)
            {
                Insert(n.Node, n.Point);
            }

            IsCollectionChangedEnabled = true;
        }

        /// <summary>
        /// Indicates if <see cref="CollectionChanged"/> will be raised on collection changes.
        /// </summary>
        protected bool IsCollectionChangedEnabled { get; set; } = true;

        /// <summary>
        /// Determines if any nodes are contained within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds to search.</param>
        /// <returns>true, if any nodes are contained within the specified bounds; otherwise, false.</returns>
        public bool ContainsNodes(RectangleF bounds)
        {
            if (root == null)
            {
                return false;
            }
            return root.ContainsNodes(bounds);
        }

        /// <summary>
        /// Returns nodes contained within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds to search.</param>
        /// <returns>The nodes contained within the specified bounds.</returns>
        public IEnumerable<T> GetNodes(RectangleF bounds)
        {
            foreach (QuadNode n in GetQuadNodes(bounds))
            {
                yield return n.Node;
            }
        }

        /// <summary>
        /// Returns nodes contained within the specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds to search.</param>
        /// <returns>The nodes contained within the specified bounds.</returns>
        IEnumerable<QuadNode> GetQuadNodes(RectangleF bounds)
        {
            var result = new List<QuadNode>();
            if (root != null)
            {
                root.GetNodes(result, bounds);
            }
            return result;
        }

        /// <summary>
        /// Inserts a node into this tree at the specified position.
        /// </summary>
        /// <param name="node">The node to insert.</param>
        /// <param name="point">The point to insert the node.</param>
        /// <exception cref="ArgumentException">
        /// <see cref="Bounds"/> must have non-zero width and height.
        /// </exception>
        public void Insert(T node, Point point)
        {
            if (Bounds.Width == 0 || Bounds.Height == 0)
                throw new ArgumentException("Bounds must have non-zero width and height.");

            if (root == null)
            {
                root = new Quadrant(null, Bounds);
            }

            Quadrant parent = root.Insert(node, point);

            table[node] = parent;
            OnCollectionChanged(NotifyCollectionChangedAction.Add, node);
        }

        /// <summary>
        /// Removes a node from this tree.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        /// <returns>true, if the node was successfully removed; otherwise, false.</returns>
        public bool Remove(T node)
        {
            if (table != null)
            {
                if (table.TryGetValue(node, out Quadrant parent))
                {
                    parent.RemoveNode(node);
                    table.Remove(node);
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove, node);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all nodes from this tree.
        /// </summary>
        public void Clear()
        {
            root = null;
            OnCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises <see cref="CollectionChanged"/> with the specified action and item.
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        /// <param name="changedItem">
        /// The item that changed due to the event. This may be null if <paramref name="action"/> is 
        /// <see cref="NotifyCollectionChangedAction.Reset"/>.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
        {
            if (IsCollectionChangedEnabled)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
            }
        }
    }
}
