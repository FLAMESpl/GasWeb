using GasWeb.Domain.Franchises;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.GasStations;
using GasWeb.Domain.Initialization;
using GasWeb.Domain.PriceSubmissions;
using GasWeb.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GasWeb.Server.Tests")]

namespace GasWeb.Domain
{
    public class GasWebBootstrap
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGasStationService, GasStationService>();
            services.AddScoped<IPriceSubmissionsService, PriceSubmissionsService>();
            services.AddScoped<IFranchiseService, FranchiseService>();
            services.AddScoped(sp => sp.GetRequiredService<SystemFranchiseCollectionFactory>().Create());
            services.AddScoped<UserContextAuditMetadataProvider>();
            services.RegisterInitializationComponents();
            services.RegisterLotosComponents();
            return services;
        }
    }
}
