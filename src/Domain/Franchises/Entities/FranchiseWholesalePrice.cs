using GasWeb.Shared;
using System;

namespace GasWeb.Domain.Franchises.Entities
{
    public class FranchiseWholesalePrice
    {
        public FranchiseWholesalePrice(long franchiseId, FuelType fuelType, decimal amount, DateTime modifiedAt)
        {
            FranchiseId = franchiseId;
            FuelType = fuelType;
            Amount = amount;
            ModifiedAt = modifiedAt;
        }

        public long FranchiseId { get; private set; }
        public FuelType FuelType { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime ModifiedAt { get; private set; }
    }
}
