using GasWeb.Domain.Comments;
using GasWeb.Shared;
using GasWeb.Shared.Comments;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GasWeb.Server.Comments
{
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;

        public CommentController(ICommentService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCommentModel model)
        {
            var id = await service.Create(model);
            return CreatedAtAction(nameof(Get), new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateCommentModel model)
        {
            await service.Update(id, model);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await service.Delete(id);
            return NoContent();
        }
             
        [HttpGet("{id:long}")]
        public Task<Comment> Get(long id)
        {
            return service.Get(id);
        }

        [HttpGet]
        public Task<PageResponse<Comment>> GetList(
            CommentTag tag,
            string subjectId,
            int pageNumber = RequestDefaults.PageNumber, 
            int pageSize = RequestDefaults.PageSize)
        {
            return service.GetList(tag, subjectId, pageNumber, pageSize);
        }
    }
}
