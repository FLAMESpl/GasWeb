using GasWeb.Domain.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace GasWeb.Server
{
    public class InitializationHostedService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public InitializationHostedService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var systemInitializer = scope.ServiceProvider.GetRequiredService<ISystemInitializer>();
                await systemInitializer.InitialzieAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
