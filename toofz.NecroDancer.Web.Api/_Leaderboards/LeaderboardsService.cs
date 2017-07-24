using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Web.Api._Leaderboards
{
    public class LeaderboardsService
    {
        public LeaderboardsService(IHttpServerUtilityWrapper server)
        {
            using (var file = File.OpenText(server.MapPath("~/App_Data/leaderboard-categories.min.json")))
            {
                var serializer = new JsonSerializer();
                Categories = ((CategoriesResponse)serializer.Deserialize(file, typeof(CategoriesResponse))).categories;
            }

            using (var file = File.OpenText(server.MapPath("~/App_Data/leaderboard-headers.min.json")))
            {
                var serializer = new JsonSerializer();
                Headers = ((Headers)serializer.Deserialize(file, typeof(Headers))).leaderboards;
            }
        }

        internal Categories Categories { get; }
        internal List<Header> Headers { get; }

        public string GetName(string category, int id)
        {
            var c = Categories[category];
            var item = c.FirstOrDefault(i => i.Value.id == id);

            return item.Key;
        }

        public string GetAll(string category)
        {
            var items = Categories[category].Select(c => c.Key);

            return string.Join(",", items);
        }

        public int? GetId(string category, string name)
        {
            return Categories[category][name]?.id;
        }
    }
}