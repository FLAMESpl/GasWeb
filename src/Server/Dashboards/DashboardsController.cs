using GasWeb.Domain.Dashboards;
using GasWeb.Shared.Dashboards.GasStations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.Dashboards
{
    [Route("api/dashboards")]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService service;

        public DashboardsController(IDashboardService service)
        {
            this.service = service;
        }

        [HttpGet("gas-stations")]
        public Task<GasStationsDashboard> GetGasStationsDashboard([FromQuery] GetGasStationsDashboard query)
        {
            return service.GetGasStations(query);
        }
    }
}
