using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    [ExcludeFromCodeCoverage]
    sealed class DailyLeaderboardConfiguration : EntityTypeConfiguration<DailyLeaderboard>
    {
        public DailyLeaderboardConfiguration()
        {
            this.HasKey(c => c.LeaderboardId);
            this.Property(c => c.LeaderboardId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
