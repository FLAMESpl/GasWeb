using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.GasStations.Lotos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.Maintenance
{
    [Route("maintenance")]
    [RequireAdminRole]
    public class MaintenanceController : ControllerBase
    {
        private readonly ILotosWholesalePriceUpdater lotosWholesalePriceUpdater;
        private readonly ILotosGasStationsUpdater lotosGasStationsUpdater;

        public MaintenanceController(
            ILotosWholesalePriceUpdater lotosWholesalePriceUpdater,
            ILotosGasStationsUpdater lotosGasStationsUpdater)
        {
            this.lotosWholesalePriceUpdater = lotosWholesalePriceUpdater;
            this.lotosGasStationsUpdater = lotosGasStationsUpdater;
        }

        [HttpPost("Lotos/refresh-prices")]
        public async Task<IActionResult> RefreshWholesalePricesForLotos()
        {
            await lotosWholesalePriceUpdater.UpdateWholesalePrices();
            return NoContent();
        }

        [HttpPost("Lotos/refresh-gas-stations")]
        public async Task<IActionResult> RefreshGasStations()
        {
            await lotosGasStationsUpdater.UpdateGasStations();
            return NoContent();
        }
    }
}
