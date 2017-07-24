namespace toofz.NecroDancer.Web.Api
{
    public static class Constants
    {
        public const string SiteName = "toofz API";
        // TODO: Switch to using environment variables
#if DEBUG
        public const string MainServer = "https://localhost:49602";
#else
        public const string MainServer = "http://crypt.toofz.com";
#endif
    }
}