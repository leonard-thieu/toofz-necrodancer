using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Replays;
using toofz.NecroDancer.Tests.Properties;

namespace toofz.NecroDancer.Tests.Data
{
    class ReplayWriterTests
    {
        [TestClass]
        public class WriteReplayDataMethod
        {
            static readonly ReplaySerializer ReplaySerializer = new ReplaySerializer();

            [TestMethod]
            public void RemoteReplay_WritesAsLocalReplay()
            {
                ReplayData remoteReplay;
                using (var s = Resources.Remote_StorySpeed.ToStream())
                {
                    remoteReplay = ReplaySerializer.Deserialize(s);
                }

                using (var ms = new MemoryStream())
                {
                    var serializer = new ReplaySerializer();
                    serializer.Serialize(ms, remoteReplay);
                    ms.Position = 0;

                    var localReplay = ReplaySerializer.Deserialize(ms);

                    Assert.IsFalse(localReplay.Header.IsRemote);
                }
            }
        }
    }
}
