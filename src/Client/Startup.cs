using Blazored.LocalStorage;
using GasWeb.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GasWeb.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<GasWebAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<GasWebAuthenticationStateProvider>());
            services.AddScoped<IAuthService, AuthService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
