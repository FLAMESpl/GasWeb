using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public static Task<ServerResponse<T>> Get<T>(this HttpClient httpClient, string url, object parameters)
            where T : class
        {
            return httpClient.Get<T>(url, new RouteValueDictionary(parameters));
        }

        public static async Task<ServerResponse<T>> Get<T>(this HttpClient httpClient, string url, RouteValueDictionary parameters = null)
            where T : class
        {
            var queryString = parameters == null ? null : GetQueryStringParameters(parameters);
            var requestUrl = string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
            var response = await httpClient.GetAsync(requestUrl);
            return await response.ToServerResponse<T>();
        }

        public static async Task<ServerResponse> SendJsonAsync(this HttpClient httpClient, HttpMethod method, string url, object content = null)
        {
            var response = await httpClient.SendJsonRequest(method, url, content);
            return await response.ToServerResponse();
        }

        public static async Task<ServerResponse<T>> SendJsonAsync<T>(this HttpClient httpClient, HttpMethod method, string url, object content = null)
            where T : class
        {
            var response = await httpClient.SendJsonRequest(method, url, content);
            return await response.ToServerResponse<T>();
        }

        public static async Task<ServerResponse> ToServerResponse(this HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode ?
                ServerResponse.Success() :
                ServerResponse.Failure(await response.GetErrors());
        }

        public static async Task<ServerResponse<T>> ToServerResponse<T>(this HttpResponseMessage response)
            where T : class
        {
            return response.IsSuccessStatusCode ?
                ServerResponse.Success(await response.Content<T>()) :
                ServerResponse.Failure<T>(await response.GetErrors());
        }

        private static async Task<string[]> GetErrors(this HttpResponseMessage response)
        {
            return response.StatusCode switch
            {
                var x when (int)x >= 500 => new[] { "Internal server error" },
                HttpStatusCode.Forbidden | HttpStatusCode.Unauthorized => new[] { "Not authorized" },
                _ => await response.Content<string[]>()
            };
        }

        private static Task<HttpResponseMessage> SendJsonRequest(this HttpClient httpClient, HttpMethod method, string url, object content = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = stringContent;
            };

            return httpClient.SendAsync(request);
        }

        private static string GetQueryStringParameters(RouteValueDictionary parameters)
        {
            return string.Join("&", parameters.Select(x => $"{x.Key}={x.Value.ToString()}"));
        }
    }
}
