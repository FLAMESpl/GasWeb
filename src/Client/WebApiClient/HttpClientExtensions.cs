using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Client.WebApiClient
{
    internal static class HttpClientExtensions
    {
        public static async Task<T> Content<T>(this HttpResponseMessage response)
        {
            var contentString = await response.Content.ReadAsStringAsync();
            var contentObject = JsonConvert.DeserializeObject<T>(contentString);
            return contentObject;
        }

        public static Task<T> Get<T>(this HttpClient httpClient, string url, object parameters)
        {
            return httpClient.Get<T>(url, new RouteValueDictionary(parameters));
        }

        public static async Task<T> Get<T>(this HttpClient httpClient, string url, RouteValueDictionary parameters = null)
        {
            var queryString = parameters == null ? null : GetQueryStringParameters(parameters);
            var requestUrl = string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
            var response = await httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content<T>();
        }

        private static string GetQueryStringParameters(RouteValueDictionary parameters)
        {
            return string.Join("&", parameters.Select(x => $"{x.Key}={x.Value.ToString()}"));
        }
    }
}
