using GasWeb.Domain.Franchises.Lotos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.Maintenance
{
    [Route("maintenance")]
    [RequireAdminRole]
    public class MaintenanceController : ControllerBase
    {
        private readonly ILotosWholesalePriceUpdater lotosWholesalePriceUpdater;

        public MaintenanceController(ILotosWholesalePriceUpdater lotosWholesalePriceUpdater)
        {
            this.lotosWholesalePriceUpdater = lotosWholesalePriceUpdater;
        }

        [HttpPost("refresh-wholesale-prices-for-lotos")]
        public async Task<IActionResult> RefreshWholesalePricesForLotos()
        {
            await lotosWholesalePriceUpdater.UpdateWholesalePrices();
            return NoContent();
        }
    }
}
