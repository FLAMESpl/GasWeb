using GasWeb.Shared.GasStations;

namespace GasWeb.Shared.Dashboards.GasStations
{
    public class GasStationsDashboardItem
    {
        public GasStationsDashboardItem(
            GasStation gasStation,
            string franchiseName,
            FuelPrices wholesalePrices,
            FuelPrices minimalSubmittedPrices,
            FuelPrices maximalSubmittedPrices)
        {
            GasStation = gasStation;
            FranchiseName = franchiseName;
            WholesalePrices = wholesalePrices;
            MinimalSubmittedPrices = minimalSubmittedPrices;
            MaximalSubmittedPrices = maximalSubmittedPrices;
        }

        public GasStation GasStation { get; }
        public string FranchiseName { get; }
        public FuelPrices WholesalePrices { get; }
        public FuelPrices MinimalSubmittedPrices { get; }
        public FuelPrices MaximalSubmittedPrices { get; }
    }
}
