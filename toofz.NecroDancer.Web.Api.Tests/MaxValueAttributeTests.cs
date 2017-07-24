using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MaxValueAttributeTests
    {
        public class IsValid
        {
            [Fact]
            public void GreaterThanMax_ReturnsFalse()
            {
                // Arrange
                var maxValueAttribute = new MaxValueAttribute(5);
                var value = 6;

                // Act
                var result = maxValueAttribute.IsValid(value);

                // Assert
                Assert.False(result);
            }

            [Theory]
            [InlineData(5)]
            [InlineData(1)]
            public void LessThanEqualMax_ReturnsTrue(int value)
            {
                // Arrange
                var maxValueAttribute = new MaxValueAttribute(5);

                // Act
                var result = maxValueAttribute.IsValid(value);

                // Assert
                Assert.True(result);
            }
        }
    }
}
