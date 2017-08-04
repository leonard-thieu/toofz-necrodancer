using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    [TestClass]
    public class TestsSetup
    {
        private static bool _wasUp;

        [AssemblyInitialize]
        public static void StartAzureBeforeAllTestsIfNotUp(TestContext context)
        {
            if (!AzureStorageEmulatorManager.IsProcessStarted())
            {
                AzureStorageEmulatorManager.StartStorageEmulator();
                _wasUp = false;
            }
            else
            {
                _wasUp = true;
            }

        }

        [AssemblyCleanup]
        public static void StopAzureAfterAllTestsIfWasDown()
        {
            if (!_wasUp)
            {
                AzureStorageEmulatorManager.StopStorageEmulator();
            }
            else
            {
                // Leave as it was before testing...
            }
        }
    }
}
