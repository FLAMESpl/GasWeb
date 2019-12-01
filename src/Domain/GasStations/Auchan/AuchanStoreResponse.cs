using System;
using System.Collections.Generic;
using System.Text;

namespace GasWeb.Domain.GasStations.Auchan
{
    internal class AuchanStoreResponse
    {
        public AuchanStoreGasStation gasstation { get; set; }
    }

    internal class AuchanStoreGasStation
    {
        public bool state { get; set; }
        public IReadOnlyCollection<GasType> gas_types { get; set; }
    }

    internal class GasType
    {
        public string name { get; set; }
        public string price { get; set; }
    }
}
