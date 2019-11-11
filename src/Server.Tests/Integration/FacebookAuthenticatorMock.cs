using GasWeb.Server.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GasWeb.Server.Tests.Integration
{
    internal class FacebookAuthenticatorMock : IFacebookAuthenticator
    {
        public Task Authenticate(HttpContext httpContext, string callback)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "Integration Test"),
                new Claim(ClaimTypes.Name, "Integration Test")
            };

            var identity = new ClaimsIdentity(claims, "TempCookie");
            var principal = new ClaimsPrincipal(identity);

            return httpContext.SignInAsync("TempCookie", principal);
        }
    }
}
