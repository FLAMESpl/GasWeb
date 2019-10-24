using System;

namespace GasWeb.Domain.GasStations.Entities
{
    internal class GasStation : AuditEntity
    {
        public GasStation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public GasStation(long id, double latitude, double longitude, long createdByUserId, long modifiedByUserId, DateTime lastModified) 
            : base(id, createdByUserId, modifiedByUserId, lastModified)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
    }
}
