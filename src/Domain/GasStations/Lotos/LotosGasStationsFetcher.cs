using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.GasStations.Lotos
{
    internal class LotosGasStationsFetcher
    {
        private const string LotosGasStationsWebPageUrl = "http://www.lotos.pl/25/dla_kierowcy/stacje_lotos?q=&r=0&f=0_0_0_0";

        private readonly HttpClient httpClient;

        public LotosGasStationsFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyList<string>> GetLotosGasStations()
        {
            var htmlDocument = await httpClient.GetHtmlDocument(LotosGasStationsWebPageUrl);
            var nodes = htmlDocument.GetElementbyId("stations-list").SelectNodes("tbody/tr/td[1]/h6/a");
            return nodes.Select(x => x.InnerText).ToList();
        }
    }
}
