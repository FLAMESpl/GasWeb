using GasWeb.Domain.Franchises.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Initialization
{
    internal class FranchiseSeeder
    {
        private static readonly IReadOnlyCollection<string> KnownFranchises = new[]
        {
            "Lotos",
            "Orlen",
            "Bp"
        };

        private readonly GasWebDbContext dbContext;
        private readonly SystemFranchiseCollectionFactory franchiseCollectionFactory;
        private readonly ILogger<FranchiseSeeder> logger;

        public FranchiseSeeder(
            GasWebDbContext dbContext, 
            SystemFranchiseCollectionFactory franchiseCollectionFactory,
            ILogger<FranchiseSeeder> logger)
        {
            this.dbContext = dbContext;
            this.franchiseCollectionFactory = franchiseCollectionFactory;
            this.logger = logger;
        }

        internal async Task SeedKnownFranchises(IAuditMetadataProvider auditMetadataProvider)
        {
            if (franchiseCollectionFactory.Initialized) return;

            var existingFranchises = await dbContext.Franchises.Where(x => KnownFranchises.Contains(x.Name)).ToListAsync();
            var missingFranchises = KnownFranchises.Except(existingFranchises.Select(x => x.Name))
                .Select(name =>
                {
                    var franchise = new Franchise(name, true);
                    auditMetadataProvider.AddAuditMetadataToNewEntity(franchise);
                    return franchise;
                })
                .ToList();

            if (missingFranchises.Any())
            {
                logger.LogInitializationProcess($"Creating franchises: { String.Join(", ", missingFranchises) }");
                dbContext.AddRange(missingFranchises);
                await dbContext.SaveChangesAsync();
            }

            var franchises = existingFranchises.Concat(missingFranchises).ToDictionary(x => x.Name, x => x.Id);
            franchiseCollectionFactory.Lotos = franchises["Lotos"];
            franchiseCollectionFactory.Orlen = franchises["Orlen"];
            franchiseCollectionFactory.Bp = franchises["Bp"];
            franchiseCollectionFactory.Initialized = true;
        }
    }
}
