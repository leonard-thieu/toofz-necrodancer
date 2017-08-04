using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamRemoteStorage;
using toofz.NecroDancer.Leaderboards.Tests.Properties;

namespace toofz.NecroDancer.Leaderboards.Tests.SteamWebApi.ISteamRemoteStorage
{
    class UgcFileDetailsTests
    {
        [TestClass]
        public class Deserialization
        {
            [TestMethod]
            public void GetUGCFileDetailsJson_DeserializesUgcFileDetails()
            {
                // Arrange
                var json = Resources.UgcFileDetails;

                // Act
                var ugcFileDetails = JsonConvert.DeserializeObject<UgcFileDetails>(json);

                // Assert
                Assert.IsInstanceOfType(ugcFileDetails, typeof(UgcFileDetails));
                Assert.IsNotNull(ugcFileDetails.Data);
            }
        }
    }

    [TestClass]
    class UgcFileDetailsDataTests
    {
        [TestClass]
        public class Deserialization
        {
            [TestMethod]
            public void GetUGCFileDetailsJson_DeserializesUgcFileDetailsData()
            {
                // Arrange
                var json = Resources.UgcFileDetails;

                // Act
                var ugcFileDetails = JsonConvert.DeserializeObject<UgcFileDetails>(json);
                var data = ugcFileDetails.Data;

                // Assert
                Assert.IsInstanceOfType(data, typeof(UgcFileDetailsData));
                Assert.AreEqual("2/9/2014_score191_zone1_level2", data.FileName);
                Assert.AreEqual("http://cloud-3.steamusercontent.com/ugc/22837952671856412/756063F4E07B686916257652BBEB972C3C9E6F8D/", data.Url);
                Assert.AreEqual(1558, data.Size);
            }
        }
    }
}
