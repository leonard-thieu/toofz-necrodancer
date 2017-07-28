using System.Collections.Generic;

namespace toofz.NecroDancer.Data
{
    public sealed class Character
    {
        public int Id { get; set; }
        public List<Item> InitialEquipment { get; } = new List<Item>();
        public List<CursedSlot> CursedSlots { get; } = new List<CursedSlot>();
    }
}
