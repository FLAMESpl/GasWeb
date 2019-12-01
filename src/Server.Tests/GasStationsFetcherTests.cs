using FluentAssertions;
using GasWeb.Domain.GasStations.Auchan;
using GasWeb.Domain.GasStations.Lotos;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests
{
    public class GasStationsFetcherTests
    {
        [Fact]
        public async Task LotosFetcher_GetGasStations_ShouldReturnAtLeastOneStation()
        {
            using var httpClient = new HttpClient();
            var fetcher = new LotosGasStationsFetcher(httpClient);
            var gasStations = await fetcher.GetLotosGasStations();

            gasStations.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task AuchanFetcher_GetGasStations_ShouldReturnAtLeastOneStation()
        {
            using var httpClient = new HttpClient();
            var fetcher = new AuchanGasStationsFetcher(httpClient);
            var gasStations = await fetcher.GetAuchanGasStations();

            gasStations.Count.Should().BeGreaterThan(0);
        }
    }
}
