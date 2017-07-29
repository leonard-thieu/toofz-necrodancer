using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MinValueAttributeTests
    {
        [TestClass]
        public class IsValid
        {
            [TestMethod]
            public void LessThanMin_ReturnsFalse()
            {
                // Arrange
                var minValueAttribute = new MinValueAttribute(5);
                var value = -1;

                // Act
                var result = minValueAttribute.IsValid(value);

                // Assert
                Assert.IsFalse(result);
            }

            [DataTestMethod]
            [DataRow(5)]
            [DataRow(32)]
            public void GreaterThanEqualMin_ReturnsTrue(int value)
            {
                // Arrange
                var minValueAttribute = new MinValueAttribute(5);

                // Act
                var result = minValueAttribute.IsValid(value);

                // Assert
                Assert.IsTrue(result);
            }
        }
    }
}
