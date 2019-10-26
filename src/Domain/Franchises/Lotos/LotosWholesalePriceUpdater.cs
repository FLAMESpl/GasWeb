using GasWeb.Domain.Franchises.Entities;
using GasWeb.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Lotos
{
    public interface ILotosWholesalePriceUpdater
    {
        Task UpdateWholesalePrices();
    }

    internal class LotosWholesalePriceUpdater : ILotosWholesalePriceUpdater
    {
        private readonly LotosWholesalePriceFetcher fetcher;
        private readonly SystemFranchiseCollection franchiseCollection;
        private readonly GasWebDbContext dbContext;

        public LotosWholesalePriceUpdater(
            LotosWholesalePriceFetcher fetcher,
            SystemFranchiseCollection franchiseCollection,
            GasWebDbContext dbContext)
        {
            this.fetcher = fetcher;
            this.franchiseCollection = franchiseCollection;
            this.dbContext = dbContext;
        }

        public async Task UpdateWholesalePrices()
        {
            IReadOnlyCollection<(FuelType fuelType, decimal amount)> prices = null;

            try
            {
                prices = await fetcher.GetPrices();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch prices from Lotos website", ex);
            }

            if (!prices.Any()) throw new Exception("Failed to fetch prices from Lotos website");

            var modifiedAt = DateTime.UtcNow;
            var wholesalePrices = prices.Select(x => new FranchiseWholesalePrice(
                franchiseId: franchiseCollection.Lotos,
                fuelType: x.fuelType,
                amount: x.amount,
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
