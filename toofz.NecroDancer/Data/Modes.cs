namespace toofz.NecroDancer.Data
{
    public abstract class Mode { }

    public sealed class HardMode : Mode
    {
        public int ExtraEnemiesPerRoom { get; set; }
        public int ExtraMinibossesPerExit { get; set; }
        public bool UpgradeStairLockingMinibosses { get; set; }
        public int MinibossesPerNonExit { get; set; }
        public bool DisableTrapdoors { get; set; }
        public bool HarderBosses { get; set; }
        public int SarcSpawnTimer { get; set; }
        public int SarcsPerRoom { get; set; }
        public bool SpawnHelperItems { get; set; }
    }
}
