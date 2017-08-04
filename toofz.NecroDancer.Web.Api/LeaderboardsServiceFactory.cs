using System;
using System.IO;
using toofz.NecroDancer.Leaderboards;

namespace toofz.NecroDancer.Web.Api
{
    public static class LeaderboardsServiceFactory
    {
        public static Categories ReadCategories()
        {
            return LeaderboardsResources.ReadCategories(MapPath("~/App_Data/leaderboard-categories.min.json"));
        }

        public static LeaderboardHeaders ReadLeaderboardHeaders()
        {
            return LeaderboardsResources.ReadLeaderboardHeaders(MapPath("~/App_Data/leaderboard-headers.min.json"));
        }

        static string MapPath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Replace("~/", ""));
        }
    }
}
