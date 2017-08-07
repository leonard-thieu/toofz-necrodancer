using toofz.NecroDancer.Leaderboards.Services.Common;

namespace toofz.NecroDancer.Leaderboards.Services.PlayersService
{
    sealed class PlayerSettings : Settings
    {
        public int PlayersPerUpdate { get; set; }
    }
}
