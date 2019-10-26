using System;
using System.Collections.Generic;
using GasWeb.Shared.Franchises;

namespace GasWeb.Domain.Franchises.Entities
{
    internal class Franchise : AuditEntity
    {
        public Franchise(string name, bool managedBySystem)
        {
            Name = name;
            ManagedBySystem = managedBySystem;
            WholesalePrices = new List<FranchiseWholesalePrice>();
        }

        public Franchise(long id, long createdByUserId, long modifiedByUserId, DateTime lastModified, string name, bool managedBySystem, 
            ICollection<FranchiseWholesalePrice> wholesalePrices)
            : base(id, createdByUserId, modifiedByUserId, lastModified)
        {
            Name = name;
            ManagedBySystem = managedBySystem;
            WholesalePrices = wholesalePrices;
        }

        public string Name { get; private set; }
        public bool ManagedBySystem { get; private set; }
        public ICollection<FranchiseWholesalePrice> WholesalePrices { get; private set; }

        internal void Update(UpdateFranchiseModel model)
        {
            Name = model.Name ?? Name;
        }
    }
}
