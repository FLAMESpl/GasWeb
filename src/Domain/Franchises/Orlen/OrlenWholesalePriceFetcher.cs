using GasWeb.Shared;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Orlen
{
    internal class OrlenWholesalePriceFetcher : IWholesalePriceProvider
    {
        private const string OrlenWholesalePriceWebPage = "https://www.orlen.pl/PL/dlabiznesu/hurtowecenypaliw/Strony/default.aspx";

        private readonly HttpClient httpClient;

        public OrlenWholesalePriceFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<FuelTypePrice>> GetPrices()
        {
            var htmlDocument = await httpClient.GetHtmlDocument(OrlenWholesalePriceWebPage);
            var diesel = GetFuelPrice(htmlDocument, "ctl00_ctl00_SPWebPartManager1_g_753cafe9_2be0_414b_aa26_6f746d63d018_ctl00_lblONEkodiselPrice");
            var petrol = GetFuelPrice(htmlDocument, "ctl00_ctl00_SPWebPartManager1_g_753cafe9_2be0_414b_aa26_6f746d63d018_ctl00_lblPb95Price");
            return new[]
            {
                new FuelTypePrice(FuelType.Diesel, diesel),
                new FuelTypePrice(FuelType.Petrol, petrol)
            };
        }

        private decimal GetFuelPrice(HtmlDocument htmlDocument, string htmlId)
        {
            var htmlNode = htmlDocument.GetElementbyId(htmlId);
            var stringAmount = htmlNode.InnerText.Replace(" ", "");
            return PriceCalculator.CalculateFromRateForCubicMeter(decimal.Parse(stringAmount));
        }
    }
}
