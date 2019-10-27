using GasWeb.Domain.Franchises.Bp;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.Franchises.Orlen;
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
        private readonly IOrlenWholesalePriceUpdater orlenWholesalePriceUpdater;
        private readonly IBpWholesalePriceUpdater bpWholesalePriceUpdater;

        public MaintenanceController(
            ILotosWholesalePriceUpdater lotosWholesalePriceUpdater,
            ILotosGasStationsUpdater lotosGasStationsUpdater,
            IOrlenWholesalePriceUpdater orlenWholesalePriceUpdater,
            IBpWholesalePriceUpdater bpWholesalePriceUpdater)
        {
            this.lotosWholesalePriceUpdater = lotosWholesalePriceUpdater;
            this.lotosGasStationsUpdater = lotosGasStationsUpdater;
            this.orlenWholesalePriceUpdater = orlenWholesalePriceUpdater;
            this.bpWholesalePriceUpdater = bpWholesalePriceUpdater;
        }

        [HttpPost("Bp/refresh-prices")]
        public async Task<IActionResult> RefreshWholesalePricesForBp()
        {
            await bpWholesalePriceUpdater.UpdateWholesalePrices();
            return NoContent();
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

        [HttpPost("Orlen/refresh-prices")]
        public async Task<IActionResult> RefreshWholesalePricesForOrlen()
        {
            await orlenWholesalePriceUpdater.UpdateWholesalePrices();
            return NoContent();
        }
    }
}
