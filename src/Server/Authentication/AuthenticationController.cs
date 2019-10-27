using GasWeb.Domain.Users;
using GasWeb.Shared.Authentication;
using GasWeb.Shared.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GasWeb.Server.Authentication
{
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthenticationSchemeProvider p;

        public AuthenticationController(IUserService userService, IAuthenticationSchemeProvider p)
        {
            this.userService = userService;
            this.p = p;
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

        [HttpGet("login-facebook")]
        public async Task LoginExternal(string callback)
        {
            await HttpContext.ChallengeAsync("Facebook", new AuthenticationProperties { RedirectUri = callback });
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
