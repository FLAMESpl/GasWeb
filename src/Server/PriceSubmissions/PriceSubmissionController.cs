using GasWeb.Domain.PriceSubmissions;
using GasWeb.Shared;
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
        [RequireUserRole]
        public async Task<IActionResult> Create([FromBody] SubmitPriceModel model)
        {
            var id = await priceSubmissionsService.SubmitPrice(model);
            return CreatedAtAction(nameof(Get), new { id });
        }

        [HttpDelete("{id:long}")]
        [RequireUserRole]
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
        public Task<PageResponse<PriceSubmission>> GetList(long? gasStationId, FuelType? fuelTypes, long? createdByUserId,
            int pageNumber = RequestDefaults.PageNumber, int pageSize = RequestDefaults.PageSize)
        {
            var types = fuelTypes ?? FuelType.Diesel | FuelType.Gas | FuelType.Petrol;
            var query = new GetPriceSubmissions(gasStationId, types, createdByUserId, pageNumber, pageSize);
            return priceSubmissionsService.GetList(query);
        }

        [HttpPut("{id:long}/rate")]
        public async Task<IActionResult> Rate(long id, [FromBody] AddPriceSubmissionRatingModel model)
        {
            await priceSubmissionsService.AddRating(id, model);
            return NoContent();
        }
    }
}
