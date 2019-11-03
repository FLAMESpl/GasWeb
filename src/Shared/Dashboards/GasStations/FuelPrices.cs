namespace GasWeb.Shared.Dashboards.GasStations
{
    public class FuelPrices
    {
        public FuelPrices(decimal? petrol, decimal? diesel)
        {
            Petrol = petrol;
            Diesel = diesel;
        }

        public decimal? Petrol { get; }
        public decimal? Diesel { get; }
    }
}
