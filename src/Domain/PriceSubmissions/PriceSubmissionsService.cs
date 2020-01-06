using GasWeb.Shared;
using GasWeb.Shared.PriceSubmissions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
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
        Task AddRating(long id, RatePriceSubmissionModel model);
    }

    internal class PriceSubmissionsService : IPriceSubmissionsService
    {
        private readonly GasWebDbContext dbContext;
        private readonly IAuditMetadataProvider auditMetadataProvider;
        private readonly UserContext userContext;

        public PriceSubmissionsService(
            GasWebDbContext dbContext, 
            IAuditMetadataProvider auditMetadataProvider,
            UserContext userContext)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
            this.userContext = userContext;
        }

        public async Task AddRating(long id, RatePriceSubmissionModel model)
        {
            var priceSubmission = await dbContext.PriceSubmissions.GetAsync(id);
            var rating = new Entities.PriceSubmissionRating(
                priceSubmissionId: priceSubmission.Id,
                userId: userContext.Id.Value,
                value: model.Value,
                submitedAt: DateTime.Now);

            await dbContext.UpsertAsync(new[] { rating }, options => options
                .SelectValues(x => new object[]
                {
                    (int)x.Value,
                    x.PriceSubmissionId,
                    $"'{x.SubmitedAt.ToString(CultureInfo.InvariantCulture)}'",
                    x.UserId
                })
                .WithColumns(
                    x => x.Value,
                    x => x.PriceSubmissionId,
                    x => x.SubmitedAt,
                    x => x.UserId
                )
                .ConflictOn(
                    x => x.UserId,
                    x => x.PriceSubmissionId
                ));
        }

        public async Task DeleteSubmission(long priceSubmissionId)
        {
            var priceSubmission = await dbContext.PriceSubmissions.GetAsync(priceSubmissionId);
            dbContext.PriceSubmissions.Remove(priceSubmission);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PriceSubmission> Get(long id)
        {
            var submission = await dbContext.PriceSubmissions.Include(x => x.Ratings).GetAsync(x => x.Id == id);
            return submission.ToContract();
        }

        public Task<PageResponse<PriceSubmission>> GetList(GetPriceSubmissions query)
        {
            var dbQuery = dbContext.PriceSubmissions.Include(x => x.Ratings).AsQueryable();

            if (query.FuelTypes != FuelType.All)
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
