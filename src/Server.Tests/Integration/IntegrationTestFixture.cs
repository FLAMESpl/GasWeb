using GasWeb.Server.Authentication;
using GasWeb.Shared.Authentication;
using GasWeb.Shared.Users;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    public class IntegrationTestFixture : IAsyncLifetime
    {
        private IWebHost webHost;
        private Task webHostTask;
        private readonly CancellationTokenSource webHostCancellationTokenSource = new CancellationTokenSource();

        public HttpClient HttpClient { get; private set; }

        public async Task DisposeAsync()
        {
            if (webHostTask != null)
            {
                webHostCancellationTokenSource.Cancel();
                await webHostTask;
            }

            webHost?.Dispose();
            HttpClient?.Dispose();
            webHostCancellationTokenSource?.Dispose();
        }

        public async Task InitializeAsync()
        {
            webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseSetting("URLS", "http://localhost:60841")
                .ConfigureServices(services =>
                {
                    services.AddScoped<IFacebookAuthenticator, FacebookAuthenticatorMock>();
                })
                .Build();

            webHostTask = webHost.RunAsync(webHostCancellationTokenSource.Token);

            var httpHandler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };

            HttpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri("http://localhost:60841/")
            };

            await CreateUserAndAuthenticate();
        }

        private async Task CreateUserAndAuthenticate()
        {
            var registerModel = new RegisterModel { Role = UserRole.Admin, Username = "Integration Test" };
            await HttpClient.GetAsync(Routes.Authentication + "/login-facebook");
            await HttpClient.PostJsonAsync(Routes.Authentication + "/register", registerModel);
        }
    }
}
