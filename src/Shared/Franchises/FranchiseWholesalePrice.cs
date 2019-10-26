using System;

namespace GasWeb.Shared.Franchises
{
    public class FranchiseWholesalePrice
    {
        public FuelType FuelType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
