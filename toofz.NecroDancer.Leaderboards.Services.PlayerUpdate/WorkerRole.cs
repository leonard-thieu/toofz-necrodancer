using System.Threading;
using System.Threading.Tasks;
using toofz.NecroDancer.Leaderboards.Services.Common;

namespace toofz.NecroDancer.Leaderboards.Services.PlayerUpdate
{
    sealed class WorkerRole : WorkerRoleBase<PlayerSettings>
    {
        public WorkerRole() : base("toofz Player Service") { }

        protected override string SettingsPath => "player-settings.json";

        protected override Task RunAsyncOverride(CancellationToken cancellationToken)
        {
            return LeaderboardsClient.UpdatePlayersAsync(Settings.PlayersPerUpdate, cancellationToken);
        }
    }
}
