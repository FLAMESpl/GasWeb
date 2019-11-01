using GasWeb.Domain.Franchises;
using GasWeb.Domain.Franchises.Entities;
using GasWeb.Domain.Schedulers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Initialization
{
    internal class SchedulerSeeder
    {
        private readonly ILogger<SchedulerSeeder> logger;
        private readonly GasWebDbContext dbContext;
        private readonly Func<SystemFranchiseCollection> franchiseCollectionFactory;

        public SchedulerSeeder(
            ILogger<SchedulerSeeder> logger,
            GasWebDbContext dbContext,
            Func<SystemFranchiseCollection> franchiseCollectionFactory)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.franchiseCollectionFactory = franchiseCollectionFactory;
        }

        public async Task SeedSchedulers(IAuditMetadataProvider auditMetadataProvider)
        {
            var franchiseCollection = franchiseCollectionFactory();
            var alreadySeededFranchises = await dbContext.Schedulers.AsNoTracking()
                .Where(x => franchiseCollection.Contains(x.FranchiseId)).Select(x => x.FranchiseId).ToListAsync();

            var franchisesToSeed = franchiseCollection.Except(alreadySeededFranchises).ToList();

            if (franchisesToSeed.Any())
            {
                logger.LogInitializationProcess("Seeding schedulers.");

                var franchises = await dbContext.Franchises.AsNoTracking().Where(x => franchisesToSeed.Contains(x.Id)).ToListAsync();
                var lotos = franchises.SingleOrDefault(x => x.Id == franchiseCollection.Lotos && !alreadySeededFranchises.Contains(x.Id));
                var orlen = franchises.SingleOrDefault(x => x.Id == franchiseCollection.Orlen);
                var bp = franchises.SingleOrDefault(x => x.Id == franchiseCollection.Bp);

                AddSchedulers(auditMetadataProvider, new[] { lotos, orlen, bp });

                await dbContext.SaveChangesAsync();
            }
        }

        private void AddSchedulers(IAuditMetadataProvider auditMetadataProvider, Franchise[] franchises)
        {
            var startedAt = DateTime.UtcNow;
            var franchisesToAdd = franchises.Where(x => x != null).ToList();
            var schedulers = franchisesToAdd
                .Select(franchise =>
                {
                    var scheduler = new Scheduler(
                        franchiseId: franchise.Id,
                        interval: TimeSpan.FromDays(1),
                        startedAt: startedAt);

                    auditMetadataProvider.AddAuditMetadataToNewEntity(scheduler);
                    return scheduler;
                })
                .ToList();

            dbContext.AddRange(schedulers);

            logger.LogInitializationProcess($"Added schedulers for { string.Join(", ", franchisesToAdd.Select(x => x.Name)) }.");
        }
    }
}
