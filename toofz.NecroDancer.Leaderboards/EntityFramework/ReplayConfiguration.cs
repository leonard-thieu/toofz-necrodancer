using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    [ExcludeFromCodeCoverage]
    sealed class ReplayConfiguration : EntityTypeConfiguration<Replay>
    {
        public ReplayConfiguration()
        {
            this.HasKey(e => e.ReplayId);
            this.Property(e => e.ReplayId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
