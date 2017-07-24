using System.Net;
using System.Text.RegularExpressions;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage.ModelDescriptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HttpStatusCodeDescription
    {
        public HttpStatusCodeDescription(HttpStatusCode statusCode, string message)
        {
            StatusCode = (int)statusCode;
            Phrase = GetPhrase(statusCode);
            if (!string.IsNullOrWhiteSpace(message) && message.Length > 2)
            {
                Documentation = "if " + char.ToLower(message[0]) + message.Substring(1);
            }
        }

        public int StatusCode { get; set; }
        public string Phrase { get; set; }
        public string Documentation { get; set; }

        private string GetPhrase(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
                return statusCode.ToString();
            return Regex.Replace(statusCode.ToString(), "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}