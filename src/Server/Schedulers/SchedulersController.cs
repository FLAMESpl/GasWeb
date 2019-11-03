using GasWeb.Domain.Schedulers;
using GasWeb.Shared;
using GasWeb.Shared.Schedulers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.Schedulers
{
    [Route("api/schedulers")]
    public class SchedulersController : ControllerBase
    {
        private readonly ISchedulerService service;

        public SchedulersController(ISchedulerService service)
        {
            this.service = service;
        }

        [HttpGet]
        [RequireModeratorRole]
        public Task<PageResponse<Scheduler>> GetList(int pageNumber = RequestDefaults.PageNumber, int pageSize = RequestDefaults.PageSize)
        {
            return service.GetList(pageNumber, pageSize);
        }

        [HttpGet("{id:long}")]
        [RequireAdminRole]
        public Task<Scheduler> Get(long id)
        {
            return service.Get(id);
        }

        [HttpPut("{id:long}")]
        [RequireAdminRole]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateSchedulerModel model)
        {
            await service.Update(id, model);
            return NoContent();
        }

        [HttpPost("{id:long}/trigger")]
        [RequireAdminRole]
        public async Task<IActionResult> Trigger(long id)
        {
            await service.Trigger(id);
            return NoContent();
        }
    }
}
