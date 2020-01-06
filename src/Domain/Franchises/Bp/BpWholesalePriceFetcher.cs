using GasWeb.Shared;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Bp
{
    internal class BpWholesalePriceFetcher : IWholesalePriceProvider
    {
        private const string BpWholesalePricesWebPage = "https://www.bp.com/pl_pl/poland/home/produkty_uslugi/hurt_paliw.html";

        private readonly HttpClient httpClient;

        public BpWholesalePriceFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<FuelTypePrice>> GetPrices()
        {
            var htmlDocument = await httpClient.GetHtmlDocument(BpWholesalePricesWebPage);
            var tableRows = htmlDocument.DocumentNode.SelectSingleNode("html/body/div[5]/div[3]/div[1]/div[1]/div[2]/div[1]/div[1]/div[2]/table/tbody");
            var petrol95 = GetPriceForFuel(tableRows, "Benzyna bezołowiowa - Pb95");
            var petrol98 = GetPriceForFuel(tableRows, "Benzyna bezołowiowa - Pb98");
            var diesel = GetPriceForFuel(tableRows, "Olej Napędowy");
            return new[]
            {
                new FuelTypePrice(FuelType.Diesel, diesel),
                new FuelTypePrice(FuelType.Pb95, petrol95),
                new FuelTypePrice(FuelType.Pb98, petrol98)
            };
        }

        private decimal GetPriceForFuel(HtmlNode tableRows, string fuel)
        {
            var row = tableRows.ChildNodes.Single(x => x.SelectSingleNode("td[1]")?.InnerText == fuel || x.SelectSingleNode("td[1]/p")?.InnerText == fuel);
            var stringPrice = row.SelectSingleNode("td[2]/p").InnerText;
            return PriceCalculator.CalculateFromRateForCubicMeter(decimal.Parse(stringPrice.Substring(0, 7)));
        }
    }
}
