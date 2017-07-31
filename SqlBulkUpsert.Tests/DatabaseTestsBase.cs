using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBulkUpsert.Tests
{
    public class DatabaseTestsBase
    {
        [ClassInitialize]
        public void Setup()
        {
            DatabaseHelper.RefreshSchema();
        }

        [TestCleanup]
        public void TearDown()
        {
            DatabaseHelper.ExecuteCommands("TRUNCATE TABLE [TestUpsert]");
        }
    }
}