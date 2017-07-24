using System;
using System.Collections.Generic;

namespace toofz.NecroDancer.Leaderboards
{
    class Headers
    {
        public List<Header> leaderboards { get; } = new List<Header>();
    }

    public class Header
    {
        public int id { get; set; }
        public string display_name { get; set; }
        public string product { get; set; }
        public string mode { get; set; }
        public string run { get; set; }
        public string character { get; set; }
    }

    class DailyHeaders
    {
        public List<DailyHeader> leaderboards { get; } = new List<DailyHeader>();
    }

    public class DailyHeader
    {
        public int id { get; set; }
        public string product { get; set; }
        public bool production { get; set; }
        public DateTime date { get; set; }
    }
}
