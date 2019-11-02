using GasWeb.Shared;
using GasWeb.Shared.GasStations;

namespace GasWeb.Domain.GasStations
{
    internal static class TypeMaps
    {
        public static GasStation ToContract(this Entities.GasStation domain)
            => new GasStation
            {
                Id = domain.Id,
                Name = domain.Name,
                AddressLine1 = domain.AddressLine1,
                AddressLine2 = domain.AddressLine2,
                CreatedByUserId = domain.CreatedByUserId,
                LastModifiedByUserId = domain.ModifiedByUserId,
                LastModifiedAt = domain.LastModified,
                ManagedBySystem = domain.MaintainedBySystem,
                FranchiseId = domain.FranchiseId
            };
    }
}
