using System;
using System.Collections.Generic;
using System.Linq;

namespace GasWeb.Shared.Franchises
{
    public class Franchise
    {
        public long Id { get; set; }
        public long CreatedByUserId { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool ManagedBySystem { get; set; }
        public IReadOnlyCollection<FranchiseWholesalePrice> WholesalePrices { get; set; }

        public decimal? FindPrice(FuelType fuelType) => WholesalePrices?.FirstOrDefault(x => x.FuelType == fuelType)?.Amount;
    }
}
