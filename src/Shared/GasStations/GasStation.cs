using System;

namespace GasWeb.Shared.GasStations
{
    public class GasStation
    {
        public long Id { get; set; }
        public Location Location { get; set; }
        public long CreatedByUserId { get; set; }
        public long LastModifiedByUserId { get; set; }
        public DateTime LastModifiedAt { get; set; }

    }
}
