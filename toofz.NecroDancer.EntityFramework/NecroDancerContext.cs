using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using toofz.NecroDancer.Data;

namespace toofz.NecroDancer.EntityFramework
{
    public class NecroDancerContext : DbContext
    {
        static NecroDancerContext()
        {
            Database.SetInitializer<NecroDancerContext>(null);
        }

        public NecroDancerContext()
        {
            Initialize();
        }

        public NecroDancerContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Initialize();
        }

        void Initialize()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbQuery<Item> Items => Set<Item>().AsNoTracking();
        public virtual DbQuery<Enemy> Enemies => Set<Enemy>().AsNoTracking();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var configs = modelBuilder.Configurations;
            configs.Add(new ItemConfiguration());
            configs.Add(new EnemyConfiguration());

            modelBuilder.ComplexType<DisplayString>();
            modelBuilder.ComplexType<Bouncer>();
            modelBuilder.ComplexType<Frame>();
            modelBuilder.ComplexType<OptionalStats>();
            modelBuilder.ComplexType<Particle>();
            modelBuilder.ComplexType<Shadow>();
            modelBuilder.ComplexType<SpriteSheet>();
            modelBuilder.ComplexType<Stats>();
            modelBuilder.ComplexType<Tweens>();
        }
    }
}
