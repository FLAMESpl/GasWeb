using System;
using GasWeb.Shared.GasStations;

namespace GasWeb.Domain.GasStations.Entities
{
    internal class GasStation : AuditEntity
    {
        public GasStation(string name, double latitude, double longitude, long? franchiseId, bool maintainedBySystem)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            FranchiseId = franchiseId;
            MaintainedBySystem = maintainedBySystem;
        }

        public GasStation(string name, long id, double latitude, double longitude, long createdByUserId, long modifiedByUserId, DateTime lastModified, long? franchiseId, bool maintainedBySystem)
            : base(id, createdByUserId, modifiedByUserId, lastModified)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            FranchiseId = franchiseId;
            MaintainedBySystem = maintainedBySystem;
        }

        public string Name { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public long? FranchiseId { get; private set; }
        public bool MaintainedBySystem { get; private set; }

        internal void Update(UpdateGasStationModel model)
        {
            Name = model.Name ?? Name;
            Latitude = model.Location?.Latitude ?? Latitude;
            Longitude = model.Location?.Longitude ?? Longitude;
        }
    }
}
