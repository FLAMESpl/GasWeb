using FluentAssertions;
using GasWeb.Domain.Franchises.Bp;
using GasWeb.Domain.Franchises.Lotos;
using GasWeb.Domain.Franchises.Orlen;
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

            prices.Should().HaveCount(4);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Pb95);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Pb98);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Diesel);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.DieselPremium);
            prices.Select(x => x.Amount).All(x => x > 2 && x < 6).Should().BeTrue();
        }

        [Fact]
        public async Task OrlenFetcher_GetPrices_ShouldReturnNonZeroPrices()
        {
            using var httpClient = new HttpClient();
            var fetcher = new OrlenWholesalePriceFetcher(httpClient);
            var prices = await fetcher.GetPrices();

            prices.Should().HaveCount(4);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Pb95);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Pb98);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Diesel);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.DieselPremium);
            prices.Select(x => x.Amount).All(x => x > 2 && x < 6).Should().BeTrue();
        }

        [Fact]
        public async Task BpFetcher_GetPrices_ShouldReturnNonZeroPrices()
        {
            using var httpClient = new HttpClient();
            var fetcher = new BpWholesalePriceFetcher(httpClient);
            var prices = await fetcher.GetPrices();

            prices.Should().HaveCount(3);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Pb95);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Pb98);
            prices.Should().ContainSingle(x => x.FuelType == FuelType.Diesel);
            prices.Select(x => x.Amount).All(x => x > 2 && x < 6).Should().BeTrue();
        }
    }
}
