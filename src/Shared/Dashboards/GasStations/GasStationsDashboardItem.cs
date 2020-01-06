using GasWeb.Shared.GasStations;
using System.Collections.Generic;

namespace GasWeb.Shared.Dashboards.GasStations
{
    public class GasStationsDashboardItem
    {
        public GasStationsDashboardItem(
            GasStation gasStation,
            string franchiseName,
            IReadOnlyCollection<FuelPrice> wholesalePrices,
            IReadOnlyCollection<FuelPrice> minimalSubmittedPrices,
            IReadOnlyCollection<FuelPrice> maximalSubmittedPrices)
        {
            GasStation = gasStation;
            FranchiseName = franchiseName;
            WholesalePrices = wholesalePrices;
            MinimalSubmittedPrices = minimalSubmittedPrices;
            MaximalSubmittedPrices = maximalSubmittedPrices;
        }

        public GasStation GasStation { get; }
        public string FranchiseName { get; }
        public IReadOnlyCollection<FuelPrice> WholesalePrices { get; }
        public IReadOnlyCollection<FuelPrice> MinimalSubmittedPrices { get; }
        public IReadOnlyCollection<FuelPrice> MaximalSubmittedPrices { get; }
    }
}
