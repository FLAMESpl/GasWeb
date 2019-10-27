using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Orlen
{
    public interface IOrlenWholesalePriceUpdater
    {
        Task UpdateWholesalePrices();
    }

    internal class OrlenWholesalePriceUpdater : WholesalePriceUpdater, IOrlenWholesalePriceUpdater
    {
        public OrlenWholesalePriceUpdater(
            OrlenWholesalePriceFetcher fetcher,
            GasWebDbContext dbContext,
            SystemFranchiseCollection franchiseCollection)
                : base(fetcher, dbContext, franchiseCollection.Orlen)
        {
        }
    }
}
