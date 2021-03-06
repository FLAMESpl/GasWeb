﻿using GasWeb.Shared;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.Franchises.Lotos
{
    internal class LotosWholesalePriceFetcher : IWholesalePriceProvider
    {
        private const string LotosWebPageUrl = "http://www.lotos.pl/144/poznaj_lotos/dla_biznesu/hurtowe_ceny_paliw";

        private readonly HttpClient httpClient;

        public LotosWholesalePriceFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<FuelTypePrice>> GetPrices()
        {
            var htmlDocument = await httpClient.GetHtmlDocument(LotosWebPageUrl);
            var tableRows = htmlDocument.DocumentNode.SelectNodes("html/body/main/section[1]/div[1]/table[1]/tbody/tr");
            var petrolPrice98 = GetPriceFromTable(tableRows, "Benzyna bezołowiowa 98");
            var petrolPrice95 = GetPriceFromTable(tableRows, "Benzyna bezołowiowa 95");
            var dieselPrice = GetPriceFromTable(tableRows, "Olej napędowy EURODIESEL");
            var dieselPricePremium = GetPriceFromTable(tableRows, "Olej napędowy I Z-40");

            return new[]
            {
                new FuelTypePrice(FuelType.Pb98, petrolPrice98),
                new FuelTypePrice(FuelType.Pb95, petrolPrice95),
                new FuelTypePrice(FuelType.Diesel, dieselPrice),
                new FuelTypePrice(FuelType.DieselPremium, dieselPricePremium)
            };
        }

        private decimal GetPriceFromTable(HtmlNodeCollection tableRows, string fuelName)
        {
            var node = tableRows.Single(x => x.SelectSingleNode("td[1]").InnerText == fuelName);
            var stringPrice = node.SelectSingleNode("td[2]").InnerText;
            var price = decimal.Parse(stringPrice.Replace(" ", ""));
            return PriceCalculator.CalculateFromRateForCubicMeter(price);
        }
    }
}
