using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Saves;

namespace toofz.NecroDancer.Tests.Data
{
    class SaveDataTests
    {
        [TestClass]
        public class LoadMethod
        {
            [TestMethod]
            public void SaveData_LoadsCorrectly()
            {
                var data = SaveData.Load("save_data.xml");

                Assert.IsNotNull(data.Player, "Player is null.");
                Assert.IsNotNull(data.Game, "Game is null.");
                Assert.IsNotNull(data.Npc, "Npc is null.");
            }
        }
    }
}
