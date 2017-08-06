using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ITargetBlockExtensionsTests
    {
        [TestClass]
        public class CheckSendAsync
        {
            [TestMethod]
            public async Task TargetIsNull_ThrowsArgumentNullException()
            {
                // Arrange -> Act
                var ex = await Record.ExceptionAsync(() =>
                {
                    return ITargetBlockExtensions.CheckSendAsync(null, (object)null);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }
    }
}
