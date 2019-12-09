using GasWeb.Domain.Franchises;
using GasWeb.Shared.PriceSubmissions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.PriceSubmissions.Auchan
{
    public interface IAuchanGasStationsPricesUpdater
    {
        Task UpdatePrices();
    }

    internal class AuchanGasStationsPricesUpdater : IAuchanGasStationsPricesUpdater
    {
        private readonly GasWebDbContext dbContext;
        private readonly AuchanGasStationsPricesFetcher fetcher;
        private readonly SystemFranchiseCollection franchiseCollection;
        private readonly IAuditMetadataProvider auditMetadataProvider;

        public AuchanGasStationsPricesUpdater(
            GasWebDbContext dbContext,
            AuchanGasStationsPricesFetcher fetcher,
            SystemFranchiseCollection franchiseCollection,
            IAuditMetadataProvider auditMetadataProvider)
        {
            this.dbContext = dbContext;
            this.fetcher = fetcher;
            this.franchiseCollection = franchiseCollection;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task UpdatePrices()
        {
            var auchanGasStations = await dbContext.GasStations
                .Where(x => franchiseCollection.Auchan == x.FranchiseId && x.WebsiteAddress != null)
                .ToListAsync();

            var prices = await fetcher.GetAuchanPrices(auchanGasStations);
            var priceSubmissions = prices.Select(ToDomain).ToList();
            dbContext.AddRange(priceSubmissions);
            await dbContext.SaveChangesAsync();
        }

        private Entities.PriceSubmission ToDomain(SubmitPriceModel contact)
        {
            var submission = new Entities.PriceSubmission(
                gasStationId: contact.GasStationId,
                submissionDate: DateTime.Now,
                amount: contact.Amount,
                fuelType: contact.FuelType);

            auditMetadataProvider.AddAuditMetadataToNewEntity(submission);
            return submission;
        }
    }
}
