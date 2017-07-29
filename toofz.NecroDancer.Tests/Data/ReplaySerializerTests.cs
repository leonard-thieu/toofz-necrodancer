using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Replays;
using toofz.NecroDancer.Tests.Properties;

namespace toofz.NecroDancer.Tests.Data
{
    class ReplaySerializerTests
    {
        [TestClass]
        public class ReadReplayMethod
        {
            static readonly ReplaySerializer ReplaySerializer = new ReplaySerializer();

            [TestMethod]
            public void CadenceWin_LoadsCorrectly()
            {
                using (var s = Resources.Local_CadenceWin.ToStream())
                {
                    var replay = ReplaySerializer.Deserialize(s);

                    Assert.AreEqual(75, replay.Header.Version);
                    Assert.IsTrue(replay.Header.LevelCount == 18);
                    Assert.IsTrue(replay.Levels.Count == 18);
                    Assert.IsNull(replay.SaveData);
                }
            }

            [TestMethod]
            public void RemoteReplay_LoadsCorrectly()
            {
                using (var s = Resources.Remote_StorySpeed.ToStream())
                {
                    var replay = ReplaySerializer.Deserialize(s);

                    Assert.AreEqual(75, replay.Header.Version);
                    Assert.IsTrue(replay.Header.LevelCount == 16);
                    Assert.IsTrue(replay.Levels.Count == 16);
                    Assert.IsNull(replay.SaveData);
                }
            }

            [TestMethod]
            public void ZeroByteReplay_ReturnsUninitialized()
            {
                using (var s = Resources.ZeroByteReplay.ToStream())
                {
                    var replay = ReplaySerializer.Deserialize(s);

                    Assert.AreEqual(0, replay.Header.Version);
                    Assert.IsTrue(replay.Levels.Count == 0);
                    Assert.IsNull(replay.SaveData);
                }
            }

            [TestMethod]
            public void KnownSeed_ReturnsCorrectSeed()
            {
                using (var s = Resources.KnownSeed_123.ToStream())
                {
                    var replay = ReplaySerializer.Deserialize(s);

                    var success = replay.TryGetSeed(out int seed);
                    Assert.IsTrue(success);
                    Assert.AreEqual(123, seed);
                }
            }
        }
    }
}
