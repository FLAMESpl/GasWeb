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

        [HttpGet("login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn()
        {
            var authResult = await HttpContext.AuthenticateAsync("TempCookie");
            if (!authResult.Succeeded)
            {
                return BadRequest(new RegisterResult 
                { 
                    Successful = false, 
                    Error = AuthenticationErrorCodes.NotAuthenticatedByExternalProvider 
                });
            }

            var nameId = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userService.FindByNameId(nameId);

            if (user != null)
            {
                await SignIn(user);
                return Ok(new LoginResult 
                { 
                    Successful = true, 
                    User = user
                });
            }
            else
            {
                return BadRequest(new LoginResult 
                { 
                    Successful = false, 
                    Error = AuthenticationErrorCodes.NotRegistered,
                    ExternalUsername = authResult.Principal.FindFirstValue(ClaimTypes.Name)
                });
            }
        }

        [HttpPost("register")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var authResult = await HttpContext.AuthenticateAsync("TempCookie");
            if (!authResult.Succeeded)
            {
                return BadRequest(new RegisterResult { Successful = false, Error = AuthenticationErrorCodes.NotAuthenticatedByExternalProvider });
            }

            var nameId = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userService.FindByNameId(nameId);

            if (user != null)
            {
                return BadRequest(new RegisterResult { Successful = false, Error = AuthenticationErrorCodes.AlreadyRegistered });
            }

            user = await userService.Add(nameId, registerModel);
            await SignIn(user);
            return Ok(new RegisterResult { Successful = true });
        }

        [HttpGet("logout")]
        //[ValidateAntiForgeryToken]
        public async Task Logout(string callback)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.ForbidAsync("Facebook", new AuthenticationProperties { RedirectUri = callback });
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

            await HttpContext.SignOutAsync("TempCookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    } 
}
