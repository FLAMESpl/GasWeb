using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Lotos
{
    public interface ILotosWholesalePriceUpdater
    {
        Task UpdateWholesalePrices();
    }

    internal class LotosWholesalePriceUpdater : WholesalePriceUpdater, ILotosWholesalePriceUpdater
    {
        public LotosWholesalePriceUpdater(
            LotosWholesalePriceFetcher fetcher,
            SystemFranchiseCollection franchiseCollection,
            GasWebDbContext dbContext)
                : base(fetcher, dbContext, franchiseCollection.Lotos)
        {
        }
    }
}
