using GasWeb.Domain.Users;
using GasWeb.Shared.Authentication;
using GasWeb.Shared.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GasWeb.Server.Authentication
{
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([FromBody] LoginModel logInModel)
        {
            var user = await userService.TryLogIn(logInModel);

            if (user != null)
            {
                await SignIn(user);
                return Ok(new LoginResult { Successful = true, User = user });
            }
            else
            {
                return BadRequest(new LoginResult { Successful = false, Error = "Invalid credentials" });
            }
        }

        [HttpPost("register")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var user = await userService.Add(registerModel);
            await SignIn(user);
            return Ok(new RegisterResult { Successful = true });
        }

        [HttpPost("logout")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [HttpGet("accessdenied")]
        public async Task<IActionResult> AccessDenied()
        {
            return Unauthorized();
        }

        private async Task SignIn(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    } 
}
