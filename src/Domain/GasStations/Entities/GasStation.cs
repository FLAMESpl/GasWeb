﻿using System;
using System.Collections.Generic;
using GasWeb.Domain.Franchises.Entities;
using GasWeb.Domain.PriceSubmissions.Entities;
using GasWeb.Shared.GasStations;

namespace GasWeb.Domain.GasStations.Entities
{
    internal class GasStation : AuditEntity
    {
        public GasStation(string name, string addressLine1, string addressLine2, long? franchiseId, bool maintainedBySystem)
        {
            Name = name;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            FranchiseId = franchiseId;
            MaintainedBySystem = maintainedBySystem;
        }

        public string Name { get; private set; }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public long? FranchiseId { get; private set; }
        public bool MaintainedBySystem { get; private set; }

        public Franchise Franchise { get; private set; }
        public ICollection<PriceSubmission> SubmitedPrices { get; private set; }

        internal void Update(UpdateGasStationModel model)
        {
            Name = model.Name ?? Name;
            AddressLine1 = model.AddressLine1 ?? AddressLine1;
            AddressLine2 = model.AddressLine2 ?? AddressLine2;
        }
    }
}
