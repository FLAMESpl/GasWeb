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
        public Task<PageResponse<Scheduler>> GetList(int pageNumber = RequestDefaults.PageNumber, int pageSize = RequestDefaults.PageSize)
        {
            return service.GetList(pageNumber, pageSize);
        }

        [HttpGet("{id:long}")]
        public Task<Scheduler> Get(long id)
        {
            return service.Get(id);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, UpdateSchedulerModel model)
        {
            await service.Update(id, model);
            return NoContent();
        }
    }
}
