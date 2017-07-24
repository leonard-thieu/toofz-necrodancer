using System;
using System.Collections.Generic;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class TypeExtensionsTests
    {
        public class GetSimpleFullName
        {
            [Fact]
            public void Returns_SimpleFullName()
            {
                // Arrange
                var type = typeof(List<object>);

                // Act
                var name = TypeExtensions.GetSimpleFullName(type);

                // Assert
                Assert.Equal("System.Collections.Generic.List`1", name);
            }

            [Fact]
            public void NullType_ThrowsArgumentNullException()
            {
                // Arrange
                Type type = null;

                // Act
                var ex = Record.Exception(() => TypeExtensions.GetSimpleFullName(type));

                // Assert
                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
            }
        }
    }
}
