namespace GasWeb.Shared.Dashboards.GasStations
{
    public class FuelPrice
    {
        public FuelPrice(FuelType type, decimal amount)
        {
            Type = type;
            Amount = amount;
        }

        public FuelType Type { get; }
        public decimal Amount { get; }
    }
}
