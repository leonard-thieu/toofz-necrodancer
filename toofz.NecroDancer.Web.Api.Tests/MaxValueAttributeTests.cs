using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MaxValueAttributeTests
    {
        [TestClass]
        public class IsValid
        {
            [TestMethod]
            public void GreaterThanMax_ReturnsFalse()
            {
                // Arrange
                var maxValueAttribute = new MaxValueAttribute(5);
                var value = 6;

                // Act
                var result = maxValueAttribute.IsValid(value);

                // Assert
                Assert.IsFalse(result);
            }

            [DataTestMethod]
            [DataRow(5)]
            [DataRow(1)]
            public void LessThanEqualMax_ReturnsTrue(int value)
            {
                // Arrange
                var maxValueAttribute = new MaxValueAttribute(5);

                // Act
                var result = maxValueAttribute.IsValid(value);

                // Assert
                Assert.IsTrue(result);
            }
        }
    }
}
