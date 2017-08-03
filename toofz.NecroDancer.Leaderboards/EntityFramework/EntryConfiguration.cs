using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    [ExcludeFromCodeCoverage]
    sealed class EntryConfiguration : EntityTypeConfiguration<Entry>
    {
        public EntryConfiguration()
        {
            this.HasKey(e => new { e.LeaderboardId, e.Rank });
            this.Property(e => e.LeaderboardId)
                .HasColumnOrder(0);
            this.Property(e => e.Rank)
                .HasColumnOrder(1);
        }
    }
}
