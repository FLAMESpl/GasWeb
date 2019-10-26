using System.Net.Http;

namespace GasWeb.Server
{
    internal class HttpClientProvider
    {
        private readonly HttpClient httpClient;

        public HttpClientProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public HttpClient Get() => httpClient;
    }
}
