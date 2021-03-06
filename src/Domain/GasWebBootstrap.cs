﻿using GasWeb.Domain.Comments;
using GasWeb.Domain.Dashboards;
using GasWeb.Domain.Franchises;
using GasWeb.Domain.Franchises.Bp;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.Franchises.Orlen;
using GasWeb.Domain.GasStations;
using GasWeb.Domain.GasStations.Auchan;
using GasWeb.Domain.GasStations.Lotos;
using GasWeb.Domain.Initialization;
using GasWeb.Domain.PriceSubmissions;
using GasWeb.Domain.PriceSubmissions.Auchan;
using GasWeb.Domain.Schedulers;
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
            services.AddScoped<ISchedulerService, SchedulerService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddSingleton<IScheduledExecutor, ScheduledExecutor>();
            services.AddScoped(sp => sp.GetRequiredService<SystemFranchiseCollectionFactory>().Create());
            services.AddScoped<IAuditMetadataProvider, UserContextAuditMetadataProvider>();
            services.AddScoped<SchedulerTaskDispatcher>();
            services.AddScoped<UserContext>();
            services.RegisterInitializationComponents();

            AddLotosComponents(services);
            AddOrlenComponents(services);
            AddBpComponents(services);
            AddAuchanComponents(services);

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

        private void AddAuchanComponents(IServiceCollection services)
        {
            services.AddScoped<IAuchanGasStationsUpdater, AuchanGasStationsUpdater>();
            services.AddScoped<AuchanGasStationsFetcher>();
            services.AddScoped<IAuchanGasStationsPricesUpdater, AuchanGasStationsPricesUpdater>();
            services.AddScoped<AuchanGasStationsPricesFetcher>();
        }
    }
}
