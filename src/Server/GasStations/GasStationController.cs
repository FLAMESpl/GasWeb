using GasWeb.Domain.GasStations;
using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.GasStations
{
    [Route("gas-stations")]
    public class GasStationController : ControllerBase
    {
        private readonly IGasStationService gasStationService;

        public GasStationController(IGasStationService gasStationService)
        {
            this.gasStationService = gasStationService;
        }

        [HttpGet]
        public Task<PageResponse<GasStation>> GetList(int pageNumber = RequestDefaults.PageNumber, int pageSize = RequestDefaults.PageSize)
        {
            return gasStationService.GetList(pageNumber, pageSize);
        }

        [HttpGet("{id:long}")]
        public Task<GasStation> Get(long id)
        {
            return gasStationService.Get(id);
        }

        [HttpPost]
        [RequireUserRole]
        public async Task<IActionResult> Create([FromBody] AddGasStationModel model)
        {
            var id = await gasStationService.Create(model);
            return CreatedAtAction(nameof(Get), new { id });
        }

        [HttpDelete("{id:long}")]
        [RequireUserRole]
        public async Task<IActionResult> Delete(long id)
        {
            await gasStationService.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id:long}")]
        [RequireUserRole]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateGasStationModel model)
        {
            await gasStationService.Update(id, model);
            return NoContent();
        }
    }
}
