using System;
using System.Collections.Generic;
using System.Drawing;

namespace toofz.NecroDancer.Dungeons
{
    partial class Quadtree<T>
    {
        internal sealed class Quadrant
        {
            public Quadrant(Quadrant parent, RectangleF bounds)
            {
                if (bounds.Width == 0 || bounds.Height == 0)
                    throw new ArgumentException("Bounds must have non-zero width and height.");

                Parent = parent;
                Bounds = bounds;
            }

            QuadNode lastNode;

            Quadrant topLeft;
            Quadrant topRight;
            Quadrant bottomLeft;
            Quadrant bottomRight;

            public Quadrant Parent { get; }

            public RectangleF Bounds { get; }

            public Quadrant Insert(T node, Point point)
            {
                Quadrant toInsert = this;
                while (true)
                {
                    var w = toInsert.Bounds.Width / 2;
                    var h = toInsert.Bounds.Height / 2;

                    var topLeft = new RectangleF(toInsert.Bounds.Left, toInsert.Bounds.Top, w, h);
                    var topRight = new RectangleF(toInsert.Bounds.Left + w, toInsert.Bounds.Top, w, h);
                    var bottomLeft = new RectangleF(toInsert.Bounds.Left, toInsert.Bounds.Top + h, w, h);
                    var bottomRight = new RectangleF(toInsert.Bounds.Left + w, toInsert.Bounds.Top + h, w, h);

                    Quadrant child = null;

                    // See if any child quadrants completely contain this node.
                    if (topLeft.Contains(point))
                    {
                        if (toInsert.topLeft == null)
                        {
                            toInsert.topLeft = new Quadrant(toInsert, topLeft);
                        }
                        child = toInsert.topLeft;
                    }
                    else if (topRight.Contains(point))
                    {
                        if (toInsert.topRight == null)
                        {
                            toInsert.topRight = new Quadrant(toInsert, topRight);
                        }
                        child = toInsert.topRight;
                    }
                    else if (bottomLeft.Contains(point))
                    {
                        if (toInsert.bottomLeft == null)
                        {
                            toInsert.bottomLeft = new Quadrant(toInsert, bottomLeft);
                        }
                        child = toInsert.bottomLeft;
                    }
                    else if (bottomRight.Contains(point))
                    {
                        if (toInsert.bottomRight == null)
                        {
                            toInsert.bottomRight = new Quadrant(toInsert, bottomRight);
                        }
                        child = toInsert.bottomRight;
                    }

                    if (child != null)
                    {
                        toInsert = child;
                    }
                    else
                    {
                        var n = new QuadNode(node, point);
                        if (toInsert.lastNode == null)
                        {
                            n.Next = n;
                        }
                        else
                        {
                            // link up in circular link list.
                            QuadNode x = toInsert.lastNode;
                            n.Next = x.Next;
                            x.Next = n;
                        }
                        toInsert.lastNode = n;
                        return toInsert;
                    }
                }
            }

            public void GetNodes(List<QuadNode> nodes, RectangleF bounds)
            {
                if (bounds.IsEmpty)
                    return;

                var w = Bounds.Width / 2;
                var h = Bounds.Height / 2;

                var tl = new RectangleF(Bounds.Left, Bounds.Top, w, h);
                var tr = new RectangleF(Bounds.Left + w, Bounds.Top, w, h);
                var bl = new RectangleF(Bounds.Left, Bounds.Top + h, w, h);
                var br = new RectangleF(Bounds.Left + w, Bounds.Top + h, w, h);

                // See if any child quadrants completely contain this node.
                if (tl.IntersectsWith(bounds) && topLeft != null)
                {
                    topLeft.GetNodes(nodes, bounds);
                }

                if (tr.IntersectsWith(bounds) && topRight != null)
                {
                    topRight.GetNodes(nodes, bounds);
                }

                if (bl.IntersectsWith(bounds) && bottomLeft != null)
                {
                    bottomLeft.GetNodes(nodes, bounds);
                }

                if (br.IntersectsWith(bounds) && bottomRight != null)
                {
                    bottomRight.GetNodes(nodes, bounds);
                }

                GetNodes(lastNode, nodes, bounds);
            }

            static void GetNodes(QuadNode last, List<QuadNode> nodes, RectangleF bounds)
            {
                if (last != null)
                {
                    QuadNode n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (bounds.Contains(n.Point))
                        {
                            nodes.Add(n);
                        }
                    } while (n != last);
                }
            }

            public bool ContainsNodes(RectangleF bounds)
            {
                if (bounds.IsEmpty)
                    return false;

                var w = Bounds.Width / 2;
                var h = Bounds.Height / 2;

                var tl = new RectangleF(Bounds.Left, Bounds.Top, w, h);
                var tr = new RectangleF(Bounds.Left + w, Bounds.Top, w, h);
                var bl = new RectangleF(Bounds.Left, Bounds.Top + h, w, h);
                var br = new RectangleF(Bounds.Left + w, Bounds.Top + h, w, h);

                bool found = false;

                // See if any child quadrants completely contain this node.
                if (tl.IntersectsWith(bounds) && topLeft != null)
                {
                    found = topLeft.ContainsNodes(bounds);
                }

                if (!found && tr.IntersectsWith(bounds) && topRight != null)
                {
                    found = topRight.ContainsNodes(bounds);
                }

                if (!found && bl.IntersectsWith(bounds) && bottomLeft != null)
                {
                    found = bottomLeft.ContainsNodes(bounds);
                }

                if (!found && br.IntersectsWith(bounds) && bottomRight != null)
                {
                    found = bottomRight.ContainsNodes(bounds);
                }

                if (!found)
                {
                    found = ContainsNodes(lastNode, bounds);
                }

                return found;
            }

            static bool ContainsNodes(QuadNode last, RectangleF bounds)
            {
                if (last != null)
                {
                    QuadNode n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (bounds.Contains(n.Point))
                        {
                            return true;
                        }
                    } while (n != last);
                }
                return false;
            }

            public bool RemoveNode(T node)
            {
                bool rc = false;
                if (lastNode != null)
                {
                    QuadNode p = lastNode;
                    while (p.Next.Node != node && p.Next != lastNode)
                    {
                        p = p.Next;
                    }
                    if (p.Next.Node == node)
                    {
                        rc = true;
                        QuadNode n = p.Next;
                        if (p == n)
                        {
                            // list goes to empty
                            lastNode = null;
                        }
                        else
                        {
                            if (lastNode == n) lastNode = p;
                            p.Next = n.Next;
                        }
                    }
                }
                return rc;
            }
        }
    }
}
