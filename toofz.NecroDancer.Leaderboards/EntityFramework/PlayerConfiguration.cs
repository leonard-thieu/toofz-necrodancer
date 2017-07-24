using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace toofz.NecroDancer.Leaderboards.EntityFramework
{
    [ExcludeFromCodeCoverage]
    internal sealed class PlayerConfiguration : EntityTypeConfiguration<Player>
    {
        public PlayerConfiguration()
        {
            this.HasKey(p => p.SteamId);
            this.Property(p => p.SteamId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Name)
                .HasMaxLength(64);
        }
    }
}
