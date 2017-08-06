using System.Threading;
using System.Threading.Tasks;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Leaderboards.Services.Common;
using toofz.NecroDancer.Leaderboards.Steam.ClientApi;

namespace toofz.NecroDancer.Leaderboards.Services.LeaderboardUpdate
{
    sealed class WorkerRole : WorkerRoleBase<Settings>
    {
        public WorkerRole() : base("toofz Leaderboard Service") { }

        protected override string SettingsPath => "leaderboard-settings.json";

        protected override async Task RunAsyncOverride(CancellationToken cancellationToken)
        {
            var userName = Util.GetEnvVar("SteamUserName");
            var password = Util.GetEnvVar("SteamPassword");
            var steamClient = new SteamClientApiClient(userName, password);
            await LeaderboardsClient.UpdateLeaderboardsAsync(steamClient, cancellationToken).ConfigureAwait(false);

            var leaderboardsConnectionString = Util.GetEnvVar("LeaderboardsConnectionString");
            using (var db = new LeaderboardsContext(leaderboardsConnectionString))
            {
                await LeaderboardsClient.UpdateDailyLeaderboardsAsync(steamClient, db, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
