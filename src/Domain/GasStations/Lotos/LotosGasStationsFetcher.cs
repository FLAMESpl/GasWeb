using GasWeb.Shared.GasStations;
using HtmlAgilityPack;
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

        public async Task<IReadOnlyList<AddGasStationModel>> GetLotosGasStations()
        {
            var htmlDocument = await httpClient.GetHtmlDocument(LotosGasStationsWebPageUrl);
            var rows = htmlDocument.GetElementbyId("stations-list").SelectNodes("tbody/tr/td[1]");
            var stations = rows.Select(GetAddModel).ToList();
            return stations;
        }

        private static AddGasStationModel GetAddModel(HtmlNode gasStationHtml)
        {
            var name = gasStationHtml.SelectSingleNode("h6/a").InnerText.Trim();
            var addressLine1 = gasStationHtml.SelectSingleNode("div/text()[1]").InnerText.Trim();
            var addressLine2 = gasStationHtml.SelectSingleNode("div/text()[2]").InnerText.Trim();

            return new AddGasStationModel
            {
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                Name = name
            };
        }
    }
}
