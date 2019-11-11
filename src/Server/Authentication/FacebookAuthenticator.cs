using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GasWeb.Server.Authentication
{
    public interface IFacebookAuthenticator
    {
        Task Authenticate(HttpContext httpContext, string callback);
    }

    internal class FacebookAuthenticator : IFacebookAuthenticator
    {
        public Task Authenticate(HttpContext httpContext, string callback)
        {
            return httpContext.ChallengeAsync("Facebook", new AuthenticationProperties { RedirectUri = callback });
        }
    }
}
