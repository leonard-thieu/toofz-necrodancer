using System;

namespace toofz.NecroDancer.Data
{
    [Flags]
    public enum OptionalStats
    {
        None = 2 << -1,
        Floating = 2 << 0,
        BounceOnMovementFail = 2 << 1,
        Phasing = 2 << 2,
        Miniboss = 2 << 3,
        Massive = 2 << 4,
        IgnoreLiquids = 2 << 5,
        Boss = 2 << 6,
    }
}
