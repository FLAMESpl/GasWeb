using Blazored.LocalStorage;
using GasWeb.Shared.Authentication;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login();
        Task Logout();
        Task<RegisterResult> Register(RegisterModel model);
    }

    internal class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly GasWebAuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthService(
            HttpClient httpClient,
            GasWebAuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
        }

        public async Task<LoginResult> Login()
        {
            var response = await httpClient.GetAsync("api/auth/login");
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResult>(stringResult);
            if (result.Successful)
            {
                var userContext = new UserContext(result.User.Id, result.User.Name, result.User.Role);
                await localStorage.SetItemAsync(userContext);
                authenticationStateProvider.MarkUserAsAuthenticated(userContext);
            }
            return result;  
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync<UserContext>();
            authenticationStateProvider.MarkUserAsLoggedOut();
        }

        public Task<RegisterResult> Register(RegisterModel model)
        {
            return httpClient.PostJsonAsync<RegisterResult>("api/auth/register", model);
        }
    }
}
