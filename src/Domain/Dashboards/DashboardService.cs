using GasWeb.Domain.Franchises.Entities;
using GasWeb.Domain.GasStations;
using GasWeb.Domain.PriceSubmissions.Entities;
using GasWeb.Shared;
using GasWeb.Shared.Dashboards.GasStations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Dashboards
{
    public interface IDashboardService
    {
        Task<GasStationsDashboard> GetGasStations(GetGasStationsDashboard query);
    }

    internal class DashboardService : IDashboardService
    {
        private readonly GasWebDbContext dbContext;

        public DashboardService(GasWebDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GasStationsDashboard> GetGasStations(GetGasStationsDashboard query)
        {
            var now = DateTime.UtcNow;
            var gasStations = await dbContext.GasStations
                .Include(x => x.SubmitedPrices)
                .ThenInclude(x => x.Ratings)
                .Include(x => x.Franchise)
                .ThenInclude(x => x.WholesalePrices)
                .PickPageAsync(query.PageNumber, query.PageSize);

            return new GasStationsDashboard(
                paging: gasStations.Paging,
                results: gasStations.Results
                    .Select(x => new GasStationsDashboardItem(
                        gasStation: x.ToContract(),
                        franchiseName: x.Franchise?.Name,
                        wholesalePrices: x.Franchise.WholesalePrices.Select(ToDashboard).ToList(),
                        minimalSubmittedPrices: GetAggregatedPricesInWindowForFuelType(x.SubmitedPrices, x => x.Min()),
                        maximalSubmittedPrices: GetAggregatedPricesInWindowForFuelType(x.SubmitedPrices, x => x.Max())
                    ))
                    .ToList());

            IReadOnlyCollection<FuelPrice> GetAggregatedPricesInWindowForFuelType(
                IEnumerable<PriceSubmission> priceSubmissions, 
                Func<IEnumerable<decimal>, decimal> aggregation)
            {
                return priceSubmissions
                    .Where(x => x.TotalScore >= 0)
                    .GroupBy(x => x.FuelType)
                    .Select(x =>
                    {
                        var prices = priceSubmissions.Where(x => now - x.SubmissionDate <= query.PastSubmittedPricesAggregationWindow).ToList();
                        var amount = prices.Any() ? 
                            aggregation(prices.Select(x => x.Amount)) :
                            priceSubmissions.OrderByDescending(x => x.SubmissionDate).First().Amount;

                        return new FuelPrice(x.Key, amount);
                    })
                    .ToList();
            }
        }

        private static FuelPrice ToDashboard(FranchiseWholesalePrice price) => new FuelPrice(price.FuelType, price.Amount);
    }
}
