using GasWeb.Domain.Franchises;
using GasWeb.Shared;
using GasWeb.Shared.Franchises;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.Franchises
{
    [Route("api/franchises")]
    public class FranchiseController : ControllerBase
    {
        private readonly IFranchiseService franchiseService;

        public FranchiseController(IFranchiseService franchiseService)
        {
            this.franchiseService = franchiseService;
        }

        [HttpGet("{id:long}")]
        public Task<Franchise> Get(long id)
        {
            return franchiseService.Get(id);
        }

        [HttpGet]
        public Task<PageResponse<Franchise>> GetList(int pageNumber = RequestDefaults.PageNumber, int pageSize = RequestDefaults.PageSize)
        {
            return franchiseService.GetList(pageNumber, pageSize);
        }

        [HttpPost]
        [RequireModeratorRole]
        public async Task<IActionResult> Create([FromBody] AddFranchiseModel model)
        {
            var id = await franchiseService.Create(model);
            return CreatedAtAction(nameof(Get), new { id });
        }

        [HttpPost("{id:long}")]
        [RequireModeratorRole]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateFranchiseModel model)
        {
            await franchiseService.Update(id, model);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        [RequireModeratorRole]
        public async Task<IActionResult> Delete(long id)
        {
            await franchiseService.Delete(id);
            return NoContent();
        }
    }
}
