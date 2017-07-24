namespace toofz.NecroDancer.Dungeons
{
    public abstract class Entity<T> : Entity
        where T : Entity
    {
        public new T Clone() => (T)base.Clone();
    }
}
