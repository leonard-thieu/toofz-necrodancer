using System.Threading;
using System.Threading.Tasks;
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
            var steamClient = new SteamClientApiClient();
            await LeaderboardsClient.UpdateLeaderboardsAsync(steamClient, cancellationToken).ConfigureAwait(false);
            await LeaderboardsClient.UpdateDailyLeaderboardsAsync(steamClient, cancellationToken).ConfigureAwait(false);
        }
    }
}
