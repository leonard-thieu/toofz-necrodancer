using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Tests.Data
{
    class ReplayDataTests
    {
        [TestClass]
        public class TryGetSeed
        {
            [TestMethod]
            public void Returns_Correct_Seed()
            {
                // Arrange
                var replayData = new ReplayData();
                var level = new LevelData
                {
                    Seed = 1580050689,
                };
                replayData.Levels.Add(level);

                // Act
                var success = replayData.TryGetSeed(out int seed);

                // Assert
                Assert.IsTrue(success);
                Assert.AreEqual(603033, seed);
            }
        }
    }
}
