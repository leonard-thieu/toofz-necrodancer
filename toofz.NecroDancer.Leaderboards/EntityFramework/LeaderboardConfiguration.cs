using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    [ExcludeFromCodeCoverage]
    sealed class LeaderboardConfiguration : EntityTypeConfiguration<Leaderboard>
    {
        public LeaderboardConfiguration()
        {
            this.HasKey(c => c.LeaderboardId);
            this.Property(c => c.LeaderboardId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Ignore(c => c.EntriesCount);
        }
    }
}
