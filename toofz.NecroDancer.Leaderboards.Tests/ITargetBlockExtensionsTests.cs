using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    class ITargetBlockExtensionsTests
    {
        [TestClass]
        public class CheckSendAsyncTests
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
