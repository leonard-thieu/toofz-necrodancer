using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class TypeExtensionsTests
    {
        [TestClass]
        public class GetSimpleFullName
        {
            [TestMethod]
            public void Returns_SimpleFullName()
            {
                // Arrange
                var type = typeof(List<object>);

                // Act
                var name = TypeExtensions.GetSimpleFullName(type);

                // Assert
                Assert.AreEqual("System.Collections.Generic.List`1", name);
            }

            [TestMethod]
            public void NullType_ThrowsArgumentNullException()
            {
                // Arrange
                Type type = null;

                // Act
                var ex = Record.Exception(() => TypeExtensions.GetSimpleFullName(type));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }
    }
}
