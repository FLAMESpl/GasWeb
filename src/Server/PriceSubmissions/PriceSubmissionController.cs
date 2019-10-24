using GasWeb.Domain.PriceSubmissions;
using GasWeb.Domain.PriceSubmissions.Queries;
using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using GasWeb.Shared.PriceSubmissions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.PriceSubmissions
{
    [Route("api/price-submissions")]
    public class PriceSubmissionController : ControllerBase
    {
        private readonly IPriceSubmissionsService priceSubmissionsService;

        public PriceSubmissionController(IPriceSubmissionsService priceSubmissionsService)
        {
            this.priceSubmissionsService = priceSubmissionsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubmitPriceModel model)
        {
            var id = await priceSubmissionsService.SubmitPrice(model);
            return CreatedAtAction(nameof(Get), new { id });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await priceSubmissionsService.DeleteSubmission(id);
            return NoContent();
        }

        [HttpGet("{id:long}")]
        public Task<PriceSubmission> Get(long id)
        {
            return priceSubmissionsService.Get(id);
        }

        [HttpGet]
        public Task<PageResponse<PriceSubmission>> GetList(long? gasStationId, FuelType? fuelType, long? createdByUserId,
            int pageNumber = RequestDefaults.PageNumber, int pageSize = RequestDefaults.PageSize)
        {
            var query = new GetPriceSubmissions(gasStationId, fuelType, createdByUserId, pageNumber, pageSize);
            return priceSubmissionsService.GetList(query);
        }
    }
}
