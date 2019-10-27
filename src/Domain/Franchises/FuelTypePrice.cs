using GasWeb.Shared;

namespace GasWeb.Domain.Franchises
{
    internal class FuelTypePrice
    {
        public FuelTypePrice(FuelType fuelType, decimal amount)
        {
            FuelType = fuelType;
            Amount = amount;
        }

        public FuelType FuelType { get; }
        public decimal Amount { get; }

        public override string ToString() => $"{ FuelType }: { Amount }";
    }
}
