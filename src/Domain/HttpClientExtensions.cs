using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain
{
    internal static class HttpClientExtensions
    {
        internal static async Task<HtmlDocument> GetHtmlDocument(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            return htmlDocument;
        }
    }
}
