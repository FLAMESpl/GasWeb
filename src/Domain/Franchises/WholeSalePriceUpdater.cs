using GasWeb.Domain.Franchises.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises
{
    internal class WholesalePriceUpdater
    {
        private readonly IWholesalePriceProvider priceProvider;
        private readonly GasWebDbContext dbContext;
        private readonly long franchiseId;

        public WholesalePriceUpdater(
            IWholesalePriceProvider priceProvider,
            GasWebDbContext dbContext,
            long franchiseId)
        {
            this.priceProvider = priceProvider;
            this.dbContext = dbContext;
            this.franchiseId = franchiseId;
        }

        public async Task UpdateWholesalePrices()
        {
            IReadOnlyCollection<FuelTypePrice> prices = null;

            try
            {
                prices = await priceProvider.GetPrices();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch prices from Lotos website", ex);
            }

            if (!prices.Any()) throw new Exception("Failed to fetch prices from Lotos website");

            var modifiedAt = DateTime.UtcNow;
            var wholesalePrices = prices.Select(x => new FranchiseWholesalePrice(
                franchiseId: franchiseId,
                fuelType: x.FuelType,
                amount: x.Amount,
                modifiedAt: modifiedAt));

            await dbContext.UpsertAsync(wholesalePrices, options => options
                .WithColumns(
                    x => x.Amount,
                    x => x.FranchiseId,
                    x => x.FuelType,
                    x => x.ModifiedAt)
                .ConflictOn(
                    x => x.FranchiseId,
                    x => x.FuelType)
                .SelectValues(p => new object[]
                {
                    p.Amount.ToString(CultureInfo.InvariantCulture),
                    p.FranchiseId,
                    (int)p.FuelType,
                    $"'{ p.ModifiedAt.ToString(CultureInfo.InvariantCulture) }'"
                }));
        }
    }
}
