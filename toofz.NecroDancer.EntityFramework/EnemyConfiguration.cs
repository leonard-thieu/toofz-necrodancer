using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.EntityFramework
{
    sealed class EnemyConfiguration : EntityTypeConfiguration<Enemy>
    {
        public EnemyConfiguration()
        {
            HasKey(e => new { e.ElementName, e.Type });
            Property(e => e.ElementName).HasColumnOrder(0);
            Property(e => e.Type).HasColumnOrder(1);

            Property(i => i.Name)
                .HasMaxLength(450)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            Ignore(e => e.FrameCount);
        }
    }
}
