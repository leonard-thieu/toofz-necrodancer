using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    public class LeaderboardsContext : DbContext
    {
        static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("LeaderboardsConnectionString", EnvironmentVariableTarget.Machine);
        }

        public LeaderboardsContext() : this(GetConnectionString()) { }

        public LeaderboardsContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbQuery<Leaderboard> Leaderboards => Set<Leaderboard>().AsNoTracking();
        public virtual DbSet<DailyLeaderboard> DailyLeaderboards => Set<DailyLeaderboard>();
        public virtual DbQuery<Entry> Entries => Set<Entry>().AsNoTracking();
        public virtual DbQuery<DailyEntry> DailyEntries => Set<DailyEntry>().AsNoTracking();
        public virtual DbQuery<Player> Players => Set<Player>().AsNoTracking();
        public virtual DbQuery<Replay> Replays => Set<Replay>().AsNoTracking();

        public virtual void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public DbRawSqlQuery<T> DailySeries<T>(DateTime date)
        {
            return Database.SqlQuery<T>(@"
WITH DailySeries
AS 
(SELECT p.LeaderboardId,
        LAG(p.Date) OVER (ORDER BY p.Date) AS Previous,
        p.Date,
        LEAD(p.Date) OVER (ORDER BY p.Date) AS Next
FROM Leaderboards AS p)

SELECT LeaderboardId,
       Previous,
       Date,
       Next
FROM DailySeries
WHERE Date = @p0;", date);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var configs = modelBuilder.Configurations;

            configs.Add(new LeaderboardConfiguration());
            configs.Add(new DailyLeaderboardConfiguration());
            configs.Add(new EntryConfiguration());
            configs.Add(new DailyEntryConfiguration());
            configs.Add(new PlayerConfiguration());
            configs.Add(new ReplayConfiguration());
        }
    }
}
