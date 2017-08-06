using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;

namespace toofz.NecroDancer.Leaderboards.Tests.Steam.ClientApi
{
    class SteamClientApiTransientErrorDetectionStrategyTests
    {
        [TestClass]
        public class IsTransient
        {
            [TestMethod]
            public void ExIsSteamClientApiException_WithTaskCanceledExceptionAsInnerException_ReturnsTrue()
            {
                // Arrange
                var strategy = new SteamClientApiTransientErrorDetectionStrategy();
                var ex = new SteamClientApiException("message", new TaskCanceledException());

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsTrue(isTransient);
            }

            [TestMethod]
            public void ExIsSteamClientApiException_WithoutTaskCanceledExceptionAsInnerException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamClientApiTransientErrorDetectionStrategy();
                var ex = new SteamClientApiException();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsFalse(isTransient);
            }

            [TestMethod]
            public void ExIsNotTaskCanceledException_ReturnsFalse()
            {
                // Arrange
                var strategy = new SteamClientApiTransientErrorDetectionStrategy();
                var ex = new Exception();

                // Act
                var isTransient = strategy.IsTransient(ex);

                // Assert
                Assert.IsFalse(isTransient);
            }
        }
    }
}
