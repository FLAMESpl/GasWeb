using GasWeb.Domain.Franchises;
using GasWeb.Domain.Franchises.Bp;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.Franchises.Orlen;
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
            AddOrlenComponents(services);
            AddBpComponents(services);

            return services;
        }

        private void AddLotosComponents(IServiceCollection services)
        {
            services.AddScoped<ILotosWholesalePriceUpdater, LotosWholesalePriceUpdater>();
            services.AddScoped<LotosWholesalePriceFetcher>();
            services.AddScoped<ILotosGasStationsUpdater, LotosGasStationsUpdater>();
            services.AddScoped<LotosGasStationsFetcher>();
        }

        private void AddOrlenComponents(IServiceCollection services)
        {
            services.AddScoped<IOrlenWholesalePriceUpdater, OrlenWholesalePriceUpdater>();
            services.AddScoped<OrlenWholesalePriceFetcher>();
        }

        private void AddBpComponents(IServiceCollection services)
        {
            services.AddScoped<IBpWholesalePriceUpdater, BpWholesalePriceUpdater>();
            services.AddScoped<BpWholesalePriceFetcher>();
        }
    }
}
