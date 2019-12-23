using GasWeb.Domain;
using GasWeb.Server.Authentication;
using GasWeb.Server.Settings;
using GasWeb.Shared.Authentication;
using GasWeb.Shared.Users;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private DbContextOptions<GasWebDbContext> dbContextOptions;

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
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseSetting("URLS", "http://localhost:60841")
                .ConfigureTestServices(services =>
                {
                    services.AddScoped<IFacebookAuthenticator, FacebookAuthenticatorMock>();
                })
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile(configPath);
                })
                .Build();

            var postgreSqlSettings = webHost.Services.GetRequiredService<PostgreSqlSettings>();
            dbContextOptions = new DbContextOptionsBuilder<GasWebDbContext>().UseNpgsql(postgreSqlSettings.ConnectionString).Options;            

            using (var dbContext = new GasWebDbContext(dbContextOptions))
            {
                await dbContext.Database.EnsureDeletedAsync();
                dbContext.Database.Migrate();
            }

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
