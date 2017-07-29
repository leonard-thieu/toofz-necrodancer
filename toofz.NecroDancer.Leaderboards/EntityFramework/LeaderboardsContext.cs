using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    public class LeaderboardsContext : DbContext
    {
        public LeaderboardsContext()
        {
            Initialize();
        }

        public LeaderboardsContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Initialize();
        }

        void Initialize()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbQuery<Leaderboard> Leaderboards => Set<Leaderboard>().AsNoTracking();
        public virtual DbQuery<Entry> Entries => Set<Entry>().AsNoTracking();
        public virtual DbSet<DailyLeaderboard> DailyLeaderboards => Set<DailyLeaderboard>();
        public virtual DbQuery<DailyEntry> DailyEntries => Set<DailyEntry>().AsNoTracking();
        public virtual DbQuery<Player> Players => Set<Player>().AsNoTracking();
        public virtual DbQuery<Replay> Replays => Set<Replay>().AsNoTracking();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var configs = modelBuilder.Configurations;

            configs.Add(new LeaderboardConfiguration());
            configs.Add(new EntryConfiguration());
            configs.Add(new DailyLeaderboardConfiguration());
            configs.Add(new DailyEntryConfiguration());
            configs.Add(new PlayerConfiguration());
            configs.Add(new ReplayConfiguration());
        }
    }
}
