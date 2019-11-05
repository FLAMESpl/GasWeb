using GasWeb.Shared;
using GasWeb.Shared.Dashboards.GasStations;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        public Task<ServerResponse<List<GasStationsDashboardItem>>> GetGasStations()
        {
            return QueryingExtensions.GetAllPages((pageNumber, pageSize) => httpClient
                .Get<GasStationsDashboard>($"{Route}/gas-stations",
                    new GetGasStationsDashboard
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    })
                .ContinueWith(x => x.Result.To<PageResponse<GasStationsDashboardItem>>()));
        }
    }
}
