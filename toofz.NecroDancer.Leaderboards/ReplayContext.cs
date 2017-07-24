using System;
using System.IO;
using System.Net.Http;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class ReplayContext
    {
        public long UgcId { get; set; }

        public int? ErrorCode { get; set; }

        public HttpContent DetailsContent { get; set; }

        public Uri DataUri { get; set; }
        public HttpContent DataContent { get; set; }

        public Stream Data { get; set; }
    }
}
