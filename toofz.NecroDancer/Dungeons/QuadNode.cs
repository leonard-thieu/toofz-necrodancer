using System.Drawing;

namespace toofz.NecroDancer.Dungeons
{
    partial class Quadtree<T>
    {
        internal sealed class QuadNode
        {
            public QuadNode(T node, Point point)
            {
                Node = node;
                Point = point;
            }

            public T Node { get; }

            public Point Point { get; }

            public QuadNode Next { get; set; }
        }
    }
}
