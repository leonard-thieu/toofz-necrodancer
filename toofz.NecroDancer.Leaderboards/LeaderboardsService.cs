using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsService
    {
        public Categories ReadCategories(string path)
        {
            using (var file = File.OpenText(path))
            {
                var serializer = new JsonSerializer();
                var response = ((CategoriesResponse)serializer.Deserialize(file, typeof(CategoriesResponse)));

                return response.categories;
            }
        }

        sealed class CategoriesResponse
        {
            public Categories categories { get; set; }
        }

        public LeaderboardHeaders ReadLeaderboardHeaders(string path)
        {
            using (var file = File.OpenText(path))
            {
                var serializer = new JsonSerializer();
                var response = ((LeaderboardHeadersResponse)serializer.Deserialize(file, typeof(LeaderboardHeadersResponse)));

                return response.leaderboards;
            }
        }

        sealed class LeaderboardHeadersResponse
        {
            public LeaderboardHeaders leaderboards { get; } = new LeaderboardHeaders();
        }

        public DailyLeaderboardHeaders ReadDailyLeaderboardHeaders(string path)
        {
            using (var file = File.OpenText(path))
            {
                var serializer = new JsonSerializer();
                var response = ((DailyLeaderboardHeadersResponse)serializer.Deserialize(file, typeof(DailyLeaderboardHeadersResponse)));

                return response.leaderboards;
            }
        }

        sealed class DailyLeaderboardHeadersResponse
        {
            public DailyLeaderboardHeaders leaderboards { get; } = new DailyLeaderboardHeaders();
        }
    }

    public sealed class Categories : Dictionary<string, Category>
    {
        public Category GetCategory(string categoryName)
        {
            if (categoryName == null)
                throw new ArgumentNullException(nameof(categoryName), $"{nameof(categoryName)} is null.");

            if (TryGetValue(categoryName, out Category c))
            {
                return c;
            }
            else
            {
                throw new ArgumentException($"'{categoryName}' is not a valid category.");
            }
        }

        public string GetItemName(string categoryName, int id)
        {
            var category = GetCategory(categoryName);
            try
            {
                var item = category.Single(i => i.Value.id == id);

                return item.Key;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"Unable to find an item with id '{id}' in '{categoryName}'.", nameof(id), ex);
            }
        }

        public string GetAllItemNames(string categoryName)
        {
            var category = GetCategory(categoryName);
            var itemNames = category.Select(c => c.Key);

            return string.Join(",", itemNames);
        }

        public int GetItemId(string categoryName, string itemName)
        {
            if (itemName == null)
                throw new ArgumentNullException(nameof(itemName), $"{nameof(itemName)} is null.");

            var category = GetCategory(categoryName);
            try
            {
                var item = category[itemName];

                return item.id;
            }
            catch (KeyNotFoundException ex)
            {
                throw new ArgumentException($"Unable to find an item with name '{itemName}' in '{categoryName}'.", nameof(itemName), ex);
            }
        }
    }

    public sealed class Category : Dictionary<string, CategoryItem> { }

    public sealed class CategoryItem
    {
        public int id { get; set; }
        public string display_name { get; set; }
    }

    public sealed class LeaderboardHeaders : Collection<LeaderboardHeader> { }

    public sealed class LeaderboardHeader
    {
        public int id { get; set; }
        public string display_name { get; set; }
        public string product { get; set; }
        public string mode { get; set; }
        public string run { get; set; }
        public string character { get; set; }
    }

    public sealed class DailyLeaderboardHeaders : Collection<DailyLeaderboardHeader> { }

    public sealed class DailyLeaderboardHeader
    {
        public int id { get; set; }
        public string product { get; set; }
        public bool production { get; set; }
        public DateTime date { get; set; }
    }
}
