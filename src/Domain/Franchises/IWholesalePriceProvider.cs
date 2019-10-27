using System.Collections.Generic;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises
{
    internal interface IWholesalePriceProvider
    {
        Task<IReadOnlyCollection<FuelTypePrice>> GetPrices();
    }
}
