using GasWeb.Domain.GasStations.Auchan;
using GasWeb.Domain.GasStations.Entities;
using GasWeb.Shared;
using GasWeb.Shared.PriceSubmissions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.PriceSubmissions.Auchan
{
    internal class AuchanGasStationsPricesFetcher
    {
        private const string AuchanStoresUrl = "https://yep.auchan.com/corp/cms/v3/pl/template/stores/";
        private const string ApiKeyHeaderName = "X-Gravitee-Api-Key";
        private const string ApiKeyValue = "18e88bf0-06e2-4470-b07b-6fe55959ac36";

        private readonly HttpClient httpClient;

        public AuchanGasStationsPricesFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<SubmitPriceModel>> GetAuchanPrices(
            IReadOnlyCollection<GasStation> gasStations)
        {
            var result = new List<SubmitPriceModel>();

            foreach (var gasStation in gasStations)
            {
                var prices = await GetPrices(gasStation);
                result.AddRange(prices);
            }

            return result;
        }

        private async Task<IReadOnlyCollection<SubmitPriceModel>> GetPrices(GasStation gasStation)
        {
            var store = await SendRequest<AuchanStoreResponse>(AuchanStoresUrl + gasStation.WebsiteAddress);
            return store?.gasstation?.state == true ? store.gasstation.gas_types?.Select(x => ToPriceSubmission(gasStation.Id, x))
                .Where(x => x.FuelType != FuelType.None).ToArray() ?? Array.Empty<SubmitPriceModel>() : Array.Empty<SubmitPriceModel>();
        }

        private async Task<T> SendRequest<T>(string url) where T : class
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequest.Headers.TryAddWithoutValidation(ApiKeyHeaderName, ApiKeyValue);
            var response = await httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var stringContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(stringContent);
            }
            else
            {
                return null;
            }
        }

        private SubmitPriceModel ToPriceSubmission(long gasStationId, GasType gasType)
        {
            return new SubmitPriceModel
            {
                Amount = decimal.Parse(gasType.price, CultureInfo.InvariantCulture),
                FuelType = gasType.name switch
                {
                    "Pb95 (E5)" => FuelType.Petrol,
                    "ON (B7)" => FuelType.Diesel,
                    "LPG" => FuelType.Gas,
                    _ => FuelType.None,
                },
                GasStationId = gasStationId
            };
        }
    }
}
