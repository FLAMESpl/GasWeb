using GasWeb.Domain.Users;
using GasWeb.Shared.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GasWeb.Server.Users
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<User>> GetList()
        {
            return userService.GetList();
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var user = await userService.Get(id);
            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }

        [HttpPatch("{id:long}")]
        [RequireModeratorRole]
        public async Task<IActionResult> Update(long id, [FromBody] UserUpdateModel updateModel)
        {
            await userService.Update(id, updateModel);
            return NoContent();
        }
    }
}
