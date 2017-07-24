using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Dungeons;

namespace toofz.NecroDancer.Tests.Dungeons
{
    public class QuadtreeTests
    {
        [TestClass]
        public class InsertMethod
        {
            Quadtree<object> qt;

            [TestInitialize]
            public void TestInitialize()
            {
                qt = new Quadtree<object> { Bounds = new RectangleF(0, 0, 100, 100) };
            }

            [TestMethod]
            public void InsertNode_ContainsNode()
            {
                var node = new object();
                var pt = new Point(0, 0);

                qt.Insert(node, pt);

                var nodes = qt.GetNodes(qt.Bounds).ToList();
                CollectionAssert.Contains(nodes, node);
            }

            [TestMethod]
            public void InsertNodes_ContainsNodes()
            {
                var node1 = new object();
                var pt1 = new Point(0, 0);
                qt.Insert(node1, pt1);
                var node2 = new object();
                var pt2 = new Point(1, 1);
                qt.Insert(node2, pt2);

                var nodes = qt.GetNodes(qt.Bounds).ToList();
                CollectionAssert.Contains(nodes, node1);
                CollectionAssert.Contains(nodes, node2);
            }

            [TestMethod]
            public void InsertNodesAtSamePoint_ContainsNodes()
            {
                var node1 = new object();
                var pt = new Point(0, 0);
                qt.Insert(node1, pt);
                var node2 = new object();
                qt.Insert(node2, pt);

                var nodes = qt.GetNodes(qt.Bounds).ToList();
                CollectionAssert.Contains(nodes, node1);
                CollectionAssert.Contains(nodes, node2);
            }
        }

        [TestClass]
        public class RemoveMethod
        {
            Quadtree<object> qt;

            [TestInitialize]
            public void TestInitialize()
            {
                qt = new Quadtree<object> { Bounds = new RectangleF(0, 0, 100, 100) };
            }

            [TestMethod]
            public void RemoveNodeFromEmptyTree_ReturnsFalse()
            {
                var node = new object();

                var result = qt.Remove(node);

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void RemoveNodeNotInTree_ReturnsFalse()
            {
                var node1 = new object();
                var pt = new Point(0, 0);
                qt.Insert(node1, pt);
                var node2 = new object();

                var result = qt.Remove(node2);

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void RemoveNodeInTree_ReturnsTrue()
            {
                var node = new object();
                var pt = new Point(0, 0);

                qt.Insert(node, pt);
                var result = qt.Remove(node);

                Assert.IsTrue(result);
            }
        }

        [TestClass]
        public class ClearMethod
        {
            Quadtree<object> qt;

            [TestInitialize]
            public void TestInitialize()
            {
                qt = new Quadtree<object> { Bounds = new RectangleF(0, 0, 100, 100) };
            }

            [TestMethod]
            public void Clear_RemovesAllNodes()
            {
                var node1 = new object();
                var pt1 = new Point(0, 0);
                qt.Insert(node1, pt1);
                var node2 = new object();
                var pt2 = new Point(1, 1);
                qt.Insert(node2, pt2);

                qt.Clear();

                var count = qt.GetNodes(qt.Bounds).Count();
                Assert.AreEqual(0, count);
            }
        }

        [TestClass]
        public class CollectionChangedEvent
        {
            Quadtree<object> qt;
            NotifyCollectionChangedAction? action;

            [TestInitialize]
            public void TestInitialize()
            {
                qt = new Quadtree<object> { Bounds = new RectangleF(0, 0, 100, 100) };
                qt.CollectionChanged += (s, e) =>
                {
                    action = e.Action;
                };
                action = null;
            }

            [TestMethod]
            public void InsertNode_RaisesEventWithAddAction()
            {
                var node = new object();
                var pt = new Point(0, 0);

                qt.Insert(node, pt);

                Assert.AreEqual(NotifyCollectionChangedAction.Add, action);
            }

            [TestMethod]
            public void RemoveNodeNotInTree_DoesNotRaiseEvent()
            {
                var node = new object();

                var result = qt.Remove(node);

                Assert.IsNull(action);
            }

            [TestMethod]
            public void RemoveNodeInTree_RaisesEventWithRemoveAction()
            {
                var node = new object();
                var pt = new Point(0, 0);

                qt.Insert(node, pt);
                qt.Remove(node);

                Assert.AreEqual(NotifyCollectionChangedAction.Remove, action);
            }

            [TestMethod]
            public void ClearTree_RaisesEventWithResetAction()
            {
                qt.Clear();

                Assert.AreEqual(NotifyCollectionChangedAction.Reset, action);
            }
        }
    }
}
