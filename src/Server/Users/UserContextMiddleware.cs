using GasWeb.Domain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GasWeb.Server.Users
{
    internal class UserContextMiddleware
    {
        private readonly RequestDelegate next;

        public UserContextMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, UserContext userContext)
        {
            var idClaimString = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaimString != null && long.TryParse(idClaimString, out var userId))
            {
                userContext.Id = userId;
            }
            await next(httpContext);
        }
    }
}
