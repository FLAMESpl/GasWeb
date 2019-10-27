using GasWeb.Domain.Franchises;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.GasStations;
using GasWeb.Domain.GasStations.Lotos;
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

            AddLotosComponents(services);

            return services;
        }

        private void AddLotosComponents(IServiceCollection services)
        {
            services.AddScoped<ILotosWholesalePriceUpdater, LotosWholesalePriceUpdater>();
            services.AddScoped<LotosWholesalePriceFetcher>();
            services.AddScoped<ILotosGasStationsUpdater, LotosGasStationsUpdater>();
            services.AddScoped<LotosGasStationsFetcher>();
        }


    }
}
