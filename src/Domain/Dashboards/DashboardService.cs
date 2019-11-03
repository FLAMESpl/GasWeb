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
                        franchiseName: x.Franchise.Name,
                        wholesalePrices: new FuelPrices(
                            petrol: x.Franchise.WholesalePrices.SingleOrDefault(x => x.FuelType == FuelType.Petrol)?.Amount,
                            diesel: x.Franchise.WholesalePrices.SingleOrDefault(x => x.FuelType == FuelType.Diesel)?.Amount),
                        minimalSubmittedPrices: GetAggregatedMinimalPrices(x.SubmitedPrices),
                        maximalSubmittedPrices: GetAggregatedMaximalPrices(x.SubmitedPrices)
                    ))
                    .ToList());

            FuelPrices GetAggregatedMinimalPrices(ICollection<PriceSubmission> priceSubmissions)
            {
                return GetAggregatedPricesInWindow(priceSubmissions, x => x.Max());
            }

            FuelPrices GetAggregatedMaximalPrices(ICollection<PriceSubmission> priceSubmissions)
            {
                return GetAggregatedPricesInWindow(priceSubmissions, x => x.Min());
            }

            FuelPrices GetAggregatedPricesInWindow(
                ICollection<PriceSubmission> priceSubmissions,
                Func<IEnumerable<decimal>, decimal> aggregation)
            {
                return new FuelPrices(
                    GetAggregatedPricesInWindowForFuelType(priceSubmissions.Where(x => x.FuelType == FuelType.Petrol), aggregation),
                    GetAggregatedPricesInWindowForFuelType(priceSubmissions.Where(x => x.FuelType == FuelType.Diesel), aggregation));
            }

            decimal? GetAggregatedPricesInWindowForFuelType(
                IEnumerable<PriceSubmission> priceSubmissions, 
                Func<IEnumerable<decimal>, decimal> aggregation)
            {
                priceSubmissions = priceSubmissions.Where(x => x.TotalScore >= 0).ToList();

                if (!priceSubmissions.Any())
                    return null;

                var prices = priceSubmissions.Where(x => now - x.SubmissionDate <= query.PastSubmittedPricesAggregationWindow).ToList();
                if (prices.Any())
                {
                    return aggregation(prices.Select(x => x.Amount));
                }
                else
                {
                    return priceSubmissions.OrderByDescending(x => x.SubmissionDate).First().Amount;
                }
            }
        }
    }
}
