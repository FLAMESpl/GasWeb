using GasWeb.Shared;
using GasWeb.Shared.PriceSubmissions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.PriceSubmissions
{
    public interface IPriceSubmissionsService
    {
        Task DeleteSubmission(long priceSubmissionId);
        Task<long> SubmitPrice(SubmitPriceModel model);
        Task<PriceSubmission> Get(long id);
        Task<PageResponse<PriceSubmission>> GetList(GetPriceSubmissions query);
    }

    internal class PriceSubmissionsService : IPriceSubmissionsService
    {
        private readonly GasWebDbContext dbContext;
        private readonly UserContextAuditMetadataProvider auditMetadataProvider;

        public PriceSubmissionsService(GasWebDbContext dbContext, UserContextAuditMetadataProvider auditMetadataProvider)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
        }

        public async Task DeleteSubmission(long priceSubmissionId)
        {
            var priceSubmission = await dbContext.PriceSubmissions.GetAsync(priceSubmissionId);
            dbContext.PriceSubmissions.Remove(priceSubmission);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PriceSubmission> Get(long id)
        {
            var submission = await dbContext.PriceSubmissions.GetAsync(id);
            return submission.ToContract();
        }

        public Task<PageResponse<PriceSubmission>> GetList(GetPriceSubmissions query)
        {
            var dbQuery = dbContext.PriceSubmissions.AsQueryable();

            if (query.FuelTypes != (FuelType.Diesel | FuelType.Gas | FuelType.Petrol))
                dbQuery = dbQuery.Where(x => query.FuelTypes.HasFlag(x.FuelType));

            if (query.CreatedByUserId.HasValue)
                dbQuery = dbQuery.Where(x => x.CreatedByUserId == query.CreatedByUserId.Value);

            if (query.GasStationId.HasValue)
                dbQuery = dbQuery.Where(x => x.GasStationId == query.GasStationId.Value);

            return dbQuery
                .OrderByDescending(x => x.LastModified)
                .PickPageAsync(query.PageNumber, query.PageSize, x => x.ToContract());
        }

        public async Task<long> SubmitPrice(SubmitPriceModel model)
        {
            var submission = new Entities.PriceSubmission(
                gasStationId: model.GasStationId,
                submissionDate: DateTime.UtcNow.Date,
                amount: model.Amount,
                fuelType: model.FuelType);

            auditMetadataProvider.AddAuditMetadataToNewEntity(submission);
            dbContext.PriceSubmissions.Add(submission);
            await dbContext.SaveChangesAsync();
            return submission.Id;
        }
    }
}
