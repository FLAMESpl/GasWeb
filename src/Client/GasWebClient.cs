using System.Net.Http;

namespace GasWeb.Client
{
    internal class GasWebClient
    {
        public GasWebClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }


    }
}
