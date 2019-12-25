using GasWeb.Shared.PriceSubmissions;
using System;
using System.Linq;

namespace GasWeb.Domain.PriceSubmissions
{
    internal static class TypeMaps
    {
        public static PriceSubmission ToContract(this Entities.PriceSubmission domain)
            => new PriceSubmission(
                id: domain.Id,
                createdByUserId: domain.CreatedByUserId,
                modifiedAt: domain.LastModified,
                gasStationId: domain.GasStationId,
                amount: domain.Amount,
                fuelType: domain.FuelType,
                ratings: domain.Ratings?.Select(x => x.ToContract()).ToArray() ?? Array.Empty<PriceSubmissionRating>());

        public static PriceSubmissionRating ToContract(this Entities.PriceSubmissionRating domain)
            => new PriceSubmissionRating
            {
                Value = domain.Value,
                UserId = domain.UserId
            };
    }
}
