using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GasWeb.Client.Services
{
    public class GasWebAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly ILogger<GasWebAuthenticationStateProvider> logger;

        public GasWebAuthenticationStateProvider(
            HttpClient httpClient, 
            ILocalStorageService localStorage,
            ILogger<GasWebAuthenticationStateProvider> logger)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userContext = await localStorage.GetItemAsync<UserContext>();
            if (userContext == null)
            {
                logger.LogInformation("Got authentication state for anonymous user");
                return GetAnonymousUser();
            }
            else
            {
                logger.LogInformation($"Got authentication state for logged user: { userContext.Id }");
                return GetAuthenticatedUser(userContext);
            }
        }

        public void MarkUserAsAuthenticated(UserContext user)
        {
            var authState = Task.FromResult(GetAuthenticatedUser(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var authState = Task.FromResult(GetAnonymousUser());
            NotifyAuthenticationStateChanged(authState);
        }

        private AuthenticationState GetAnonymousUser() => GetAuthenticationStateForClaims();

        private AuthenticationState GetAuthenticatedUser(UserContext user) =>
            GetAuthenticationStateForClaims(
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()));

        private AuthenticationState GetAuthenticationStateForClaims(params Claim[] claims) =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims)));

    }
}