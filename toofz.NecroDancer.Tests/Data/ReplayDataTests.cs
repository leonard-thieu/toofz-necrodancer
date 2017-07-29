using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Replays;

namespace toofz.NecroDancer.Tests.Data
{
    class ReplayDataTests
    {
        [TestClass]
        public class TryGetSeed
        {
            [DataTestMethod]
            [DataRow(1580050689, 603033)]
            [DataRow(-2147483141, 89527)]
            public void Returns_Correct_Seed(int levelSeed, int seed)
            {
                // Arrange
                var replayData = new ReplayData();
                var level = new LevelData
                {
                    Seed = levelSeed,
                };
                replayData.Levels.Add(level);

                // Act
                var success = replayData.TryGetSeed(out int result);

                // Assert
                Assert.IsTrue(success);
                Assert.AreEqual(seed, result);
            }
        }
    }
}
