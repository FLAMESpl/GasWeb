using System.Net.Http;

namespace GasWeb.Client.WebApiClient
{
    public class DashboardsEndpoints
    {
        public const string Route = "api/dashboards";
        private readonly HttpClient httpClient;

        public DashboardsEndpoints(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}
