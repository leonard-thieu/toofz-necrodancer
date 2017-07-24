using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api._Leaderboards
{
    class CategoriesResponse
    {
        public Categories categories { get; set; }
    }

    class Categories : Dictionary<string, Category> { }

    class Category : Dictionary<string, Item> { }

    class Item
    {
        public int id { get; set; }
        public string display_name { get; set; }
    }

    class Headers
    {
        public List<Header> leaderboards { get; set; }
    }

    class Header
    {
        public int id { get; set; }
        public string display_name { get; set; }
        public string product { get; set; }
        public string mode { get; set; }
        public string run { get; set; }
        public string character { get; set; }
    }
}