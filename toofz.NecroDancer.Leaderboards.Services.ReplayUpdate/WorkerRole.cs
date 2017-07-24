using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using toofz.NecroDancer.Leaderboards.Services.Common;

namespace toofz.NecroDancer.Leaderboards.Services.ReplayUpdate
{
    internal sealed class WorkerRole : WorkerRoleBase<ReplaySettings>
    {
        private static CloudBlobDirectory GetDirectory(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("crypt");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container.GetDirectoryReference("replays");
        }

        public WorkerRole() : base("toofz Replay Service") { }
        protected override string SettingsPath => "replay-settings.json";

        protected override Task RunAsyncOverride(CancellationToken cancellationToken)
        {
            var storageConnectionString = Util.GetEnvVar("toofzStorageConnectionString");
            var directory = GetDirectory(storageConnectionString);

            return LeaderboardsClient.UpdateReplaysAsync(Settings.ReplaysPerUpdate, directory, cancellationToken);
        }
    }
}
