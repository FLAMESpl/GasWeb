using GasWeb.Shared.Franchises;
using System.Linq;

namespace GasWeb.Domain.Franchises
{
    internal static class TypeMaps
    {
        public static Franchise ToContract(this Entities.Franchise domain)
            => new Franchise
            {
                CreatedByUserId = domain.CreatedByUserId,
                Id = domain.Id,
                ModifiedAt = domain.LastModified,
                Name = domain.Name,
                ManagedBySystem = domain.ManagedBySystem,
                WholesalePrices = domain.WholesalePrices?.Select(x => x.ToContract()).ToList()
            };

        public static FranchiseWholesalePrice ToContract(this Entities.FranchiseWholesalePrice domain)
            => new FranchiseWholesalePrice
            {
                Amount = domain.Amount,
                FuelType = domain.FuelType,
                ModifiedAt = domain.ModifiedAt
            };
    }
}
