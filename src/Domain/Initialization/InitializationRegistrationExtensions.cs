using GasWeb.Domain.Franchises;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GasWeb.Domain.Initialization
{
    internal static class InitializationRegistrationExtensions
    {
        internal static void RegisterInitializationComponents(this IServiceCollection services)
        {
            services.AddScoped<ISystemInitializer, SystemInitializer>();
            services.AddScoped<SystemUserSeeder>();
            services.AddScoped<FranchiseSeeder>();
            services.AddScoped(sp => new SchedulerSeeder(
                logger: sp.GetRequiredService<ILogger<SchedulerSeeder>>(),
                dbContext: sp.GetRequiredService<GasWebDbContext>(),
                franchiseCollectionFactory: sp.GetRequiredService<SystemFranchiseCollection>));
            services.AddSingleton<SystemFranchiseCollectionFactory>();
        }
    }
}
