using System;

namespace GasWeb.Shared.Dashboards.GasStations
{
    public class GetGasStationsDashboard
    {
        public TimeSpan PastSubmittedPricesAggregationWindow { get; set; } = TimeSpan.FromDays(14);
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
