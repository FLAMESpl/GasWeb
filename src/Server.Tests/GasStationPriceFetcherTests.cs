using FluentAssertions;
using GasWeb.Domain.GasStations.Entities;
using GasWeb.Domain.PriceSubmissions.Auchan;
using GasWeb.Shared;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests
{
    public class GasStationPriceFetcherTests
    {
        [Fact]
        public async Task AuchanFetcher_GetStationPrice_ShouldReturnPricesForFourTypes()
        {
            using var httpClient = new HttpClient();
            var fetcher = new AuchanGasStationsPricesFetcher(httpClient);
            var station = new GasStation(default, default, default, default, default, "Auchan-Gliwice");
            var prices = await fetcher.GetAuchanPrices(new[] { station });
            prices.Select(x => x.FuelType).Should().BeEquivalentTo(new[]
            {
                FuelType.Diesel,
                FuelType.Gas,
                FuelType.Pb95,
                FuelType.Pb98
            });

            prices.Select(x => x.Amount).All(x => x > 2 && x < 6).Should().BeTrue();
        }
    }
}
