using GasWeb.Domain.Franchises;
using System;

namespace GasWeb.Domain.Initialization
{
    internal class SystemFranchiseCollectionFactory
    {
        public bool Initialized { get; set; } = false;
        public long Lotos { get; set; }

        public SystemFranchiseCollection Create()
        {
            if (!Initialized) throw new Exception("System franchises are not initialzied.");

            return new SystemFranchiseCollection(Lotos);
        }
    }
}
