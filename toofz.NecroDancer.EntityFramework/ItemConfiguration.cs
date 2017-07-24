using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.EntityFramework
{
    internal sealed class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public ItemConfiguration()
        {
            HasKey(i => i.ElementName);

            Property(i => i.Name)
                .HasMaxLength(450)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
        }
    }
}
