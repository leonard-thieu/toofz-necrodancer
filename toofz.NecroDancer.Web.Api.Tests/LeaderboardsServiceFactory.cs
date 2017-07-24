using System;
using System.IO;
using toofz.NecroDancer.Web.Api._Leaderboards;

namespace toofz.NecroDancer.Web.Api.Tests
{
    class LeaderboardsServiceFactory
    {
        public static LeaderboardsService Create()
        {
            return new LeaderboardsService(new FakeHttpServerUtilityWrapper());
        }
    }

    class FakeHttpServerUtilityWrapper : IHttpServerUtilityWrapper
    {
        public string MapPath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Replace("~/", ""));
        }
    }
}
