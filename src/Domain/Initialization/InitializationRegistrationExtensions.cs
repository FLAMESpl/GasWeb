using Microsoft.Extensions.DependencyInjection;

namespace GasWeb.Domain.Initialization
{
    internal static class InitializationRegistrationExtensions
    {
        internal static void RegisterInitializationComponents(this IServiceCollection services)
        {
            services.AddScoped<ISystemInitializer, SystemInitializer>();
            services.AddScoped<SystemUserSeeder>();
            services.AddScoped<FranchiseSeeder>();
            services.AddSingleton<SystemFranchiseCollectionFactory>();
        }
    }
}
