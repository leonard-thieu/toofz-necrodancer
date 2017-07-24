using System.ComponentModel;

namespace toofz.NecroDancer.Web.Api.Models
{
    public class ItemsPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(10)]
        public int limit { get; set; } = 10;
    }

    public class EnemiesPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(10)]
        public int limit { get; set; } = 10;
    }

    public class LeaderboardsPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(20)]
        public int limit { get; set; } = 20;
    }

    public class PlayersPagination
    {
        [MinValue(0)]
        [MaxValue(int.MaxValue)]
        [DefaultValue(0)]
        public int offset { get; set; } = 0;
        [MinValue(1)]
        [MaxValue(100)]
        [DefaultValue(100)]
        public int limit { get; set; } = 100;
    }
}