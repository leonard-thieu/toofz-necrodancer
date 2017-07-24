using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;

namespace toofz.NecroDancer.Web.Api.Areas.HelpPage
{
    [ExcludeFromCodeCoverage]
    public static class ApiDescriptionExtensions
    {
        /// <summary>
        /// Generates an URI-friendly ID for the <see cref="ApiDescription"/>. E.g. "Get-Values-id_name" instead of "GetValues/{id}?name={name}"
        /// </summary>
        /// <param name="description">The <see cref="ApiDescription"/>.</param>
        /// <returns>The ID as a string.</returns>
        public static string GetFriendlyId(this ApiDescription description)
        {
            string path = description.RelativePath;
            string[] urlParts = path.Split('?');
            string localPath = urlParts[0];
            string queryKeyString = null;
            if (urlParts.Length > 1)
            {
                string query = urlParts[1];
                string[] queryKeys = HttpUtility.ParseQueryString(query).AllKeys;
                queryKeyString = String.Join("_", queryKeys);
            }

            StringBuilder friendlyPath = new StringBuilder();
            friendlyPath.AppendFormat("{0}-{1}",
                description.HttpMethod.Method,
                localPath.Replace("/", "-").Replace("{", String.Empty).Replace("}", String.Empty));
            if (queryKeyString != null)
            {
                friendlyPath.AppendFormat("_{0}", queryKeyString.Replace('.', '-'));
            }
            return friendlyPath.ToString();
        }

        public static bool AuthorizationRequired(this ApiDescription description)
        {
            var actionDescriptor = description.ActionDescriptor;
            var isAnonymous = actionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                              actionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            return !isAnonymous && actionDescriptor.GetFilterPipeline().Where(f => f.Instance is IAuthorizationFilter).Any();
        }

        public static string GetSimpleRequestPath(this ApiDescription description)
        {
            var uri = new Uri($"http://localhost/{description.RelativePath}", UriKind.Absolute);

            return uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
        }
    }
}