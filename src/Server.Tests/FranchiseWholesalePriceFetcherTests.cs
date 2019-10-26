using FluentAssertions;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Shared;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests
{
    public class FranchiseWholesalePriceFetcherTests
    {
        [Fact]
        public async Task LotosFetcher_GetPrices_ShouldReturnNonZeroPrices()
        {
            using var httpClient = new HttpClient();
            var fetcher = new LotosWholesalePriceFetcher(httpClient);
            var prices = await fetcher.GetPrices();

            prices.Should().HaveCount(2);
            prices.Should().ContainSingle(x => x.fuelType == FuelType.Petrol);
            prices.Should().ContainSingle(x => x.fuelType == FuelType.Diesel);
            prices.Select(x => x.amount).All(x => x > 3000M && x < 6000M).Should().BeTrue();
        }
    }
}
