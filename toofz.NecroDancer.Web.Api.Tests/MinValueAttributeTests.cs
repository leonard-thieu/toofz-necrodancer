using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MinValueAttributeTests
    {
        public class IsValid
        {
            [Fact]
            public void LessThanMin_ReturnsFalse()
            {
                // Arrange
                var minValueAttribute = new MinValueAttribute(5);
                var value = -1;

                // Act
                var result = minValueAttribute.IsValid(value);

                // Assert
                Assert.False(result);
            }

            [Theory]
            [InlineData(5)]
            [InlineData(32)]
            public void GreaterThanEqualMin_ReturnsTrue(int value)
            {
                // Arrange
                var minValueAttribute = new MinValueAttribute(5);

                // Act
                var result = minValueAttribute.IsValid(value);

                // Assert
                Assert.True(result);
            }
        }
    }
}
