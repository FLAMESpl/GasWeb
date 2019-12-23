using GasWeb.Domain.Franchises;
using GasWeb.Domain.Schedulers;
using GasWeb.Domain.Schedulers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Initialization
{
    internal class SchedulerSeeder
    {
        private readonly ILogger<SchedulerSeeder> logger;
        private readonly GasWebDbContext dbContext;
        private readonly Func<SystemFranchiseCollection> franchiseCollectionFactory;
        private readonly IAuditMetadataProvider auditMetadataProvider;
        private static readonly IReadOnlyCollection<long> AllSchedulers = new[]
        {
            SchedulersCollection.RefreshPricesLotos,
            SchedulersCollection.RefreshPricesOrlen,
            SchedulersCollection.RefreshPricesBp,
            SchedulersCollection.RefreshGasStationsLotos,
            SchedulersCollection.RefreshGasStationsAuchan,
            SchedulersCollection.RefreshPricesAuchan
        };

        public SchedulerSeeder(
            ILogger<SchedulerSeeder> logger,
            GasWebDbContext dbContext,
            Func<SystemFranchiseCollection> franchiseCollectionFactory,
            IAuditMetadataProvider auditMetadataProvider)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.franchiseCollectionFactory = franchiseCollectionFactory;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task SeedSchedulers()
        {
            var franchiseCollection = franchiseCollectionFactory();
            var existingSchedulers = await dbContext.Schedulers.AsNoTracking().Select(x => x.Id).ToListAsync();
            var missingSchedulers = AllSchedulers.Except(existingSchedulers).ToList();

            if (missingSchedulers.Any())
            {
                logger.LogInitializationProcess($"Seeding schedulers: { string.Join(",", missingSchedulers) }.");

                foreach (var schedulerId in missingSchedulers)
                {
                    var scheduler = schedulerId switch
                    {
                        0 => CreateForWholesalePrices(schedulerId, franchiseCollection.Lotos),
                        1 => CreateForWholesalePrices(schedulerId, franchiseCollection.Orlen),
                        2 => CreateForWholesalePrices(schedulerId, franchiseCollection.Bp),
                        3 => CreateForGasStations(schedulerId, franchiseCollection.Lotos),
                        4 => CreateForGasStations(schedulerId, franchiseCollection.Auchan),
                        5 => CreateForGasStationPrices(schedulerId, franchiseCollection.Auchan),
                        _ => null
                    };

                    auditMetadataProvider.AddAuditMetadataToNewEntity(scheduler);
                    dbContext.Add(scheduler);
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private Scheduler CreateForWholesalePrices(long id, long franchiseId) =>
            new Scheduler(
                id: id,
                type: Shared.Schedulers.SchedulerType.RefreshWholesalePrices,
                franchiseId: franchiseId,
                interval: TimeSpan.FromSeconds(15),
                startedAt: null,
                lastRun: null);

        private Scheduler CreateForGasStations(long id, long franchiseId) =>
            new Scheduler(
                id: id,
                type: Shared.Schedulers.SchedulerType.RefreshGasStations,
                franchiseId: franchiseId,
                interval: TimeSpan.FromSeconds(30),
                startedAt: null,
                lastRun: null);

        private Scheduler CreateForGasStationPrices(long id, long franchiseId) =>
            new Scheduler(
                id: id,
                type: Shared.Schedulers.SchedulerType.RefreshWholesalePrices,
                franchiseId: franchiseId,
                interval: TimeSpan.FromDays(1),
                startedAt: null,
                lastRun: null);
    }
}
