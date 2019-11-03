using System.Collections.Generic;

namespace GasWeb.Shared.Dashboards.GasStations
{
    public class GasStationsDashboard : PageResponse<GasStationsDashboardItem>
    {
        public GasStationsDashboard(IReadOnlyCollection<GasStationsDashboardItem> results, PagingInfo paging) : base(results, paging)
        {
        }
    }
}
