using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    internal static class MockedRequestExtensions
    {
        public static void RespondJson(this MockedRequest source, object content)
        {
            source.Respond("application/json", JsonConvert.SerializeObject(content));
        }
    }
}
