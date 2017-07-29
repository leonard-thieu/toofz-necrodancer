using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.Tests.Data
{
    class NecroDancerDataTests
    {
        [TestClass]
        public class LoadMethod
        {
            [TestMethod]
            public void GameData_LoadsCorrectly()
            {
                var data = NecroDancerDataSerializer.Read("necrodancer.xml");

                Assert.AreEqual(214, data.Items.Count);
                Assert.AreEqual(163, data.Enemies.Count);
            }
        }
    }
}
