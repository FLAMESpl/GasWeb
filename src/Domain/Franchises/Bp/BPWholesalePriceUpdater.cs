using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Bp
{
    public interface IBpWholesalePriceUpdater
    {
        Task UpdateWholesalePrices();
    }

    internal class BpWholesalePriceUpdater : WholesalePriceUpdater, IBpWholesalePriceUpdater
    {
        public BpWholesalePriceUpdater(
            BpWholesalePriceFetcher priceProvider, 
            GasWebDbContext dbContext, 
            SystemFranchiseCollection franchiseCollection) 
                : base(priceProvider, dbContext, franchiseCollection.Bp)
        {
        }
    }
}
