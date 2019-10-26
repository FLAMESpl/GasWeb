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
                Location = new Location
                {
                    Longitude = domain.Longitude,
                    Latitude = domain.Latitude
                },
                CreatedByUserId = domain.CreatedByUserId,
                LastModifiedByUserId = domain.ModifiedByUserId,
                LastModifiedAt = domain.LastModified,
                ManagedBySystem = domain.MaintainedBySystem,
                FranchiseId = domain.FranchiseId
            };
    }
}
