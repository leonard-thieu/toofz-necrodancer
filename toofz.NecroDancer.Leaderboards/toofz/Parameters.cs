using System;

namespace toofz.NecroDancer.Leaderboards.toofz
{
    public sealed class GetPlayersParams : IPagination
    {
        public string Query { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string Sort { get; set; }
    }

    public sealed class GetReplaysParams : IPagination
    {
        public int? Version { get; set; }
        public int? ErrorCode { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }

    public interface IPagination
    {
        int? Offset { get; set; }
        int? Limit { get; set; }
    }
}
