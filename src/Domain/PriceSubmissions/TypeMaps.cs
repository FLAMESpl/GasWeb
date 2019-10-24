using GasWeb.Shared.GasStations;
using GasWeb.Shared.PriceSubmissions;

namespace GasWeb.Domain.PriceSubmissions
{
    internal static class TypeMaps
    {
        public static PriceSubmission ToContract(this Entities.PriceSubmission domain)
            => new PriceSubmission(
                id: domain.Id,
                createdByUserId: domain.CreatedByUserId,
                submissionDate: domain.SubmissionDate,
                gasStationId: domain.GasStationId,
                amount: domain.Amount,
                fuelType: domain.FuelType);
    }
}
