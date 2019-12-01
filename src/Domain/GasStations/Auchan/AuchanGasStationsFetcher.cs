using GasWeb.Shared.GasStations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Domain.GasStations.Auchan
{
    internal class AuchanGasStationsFetcher
    {
        private const string AuchanStoresUrl = "https://yep.auchan.com/corp/cms/v3/pl/template/stores";
        private const string ApiKeyHeaderName = "X-Gravitee-Api-Key";
        private const string ApiKeyValue = "18e88bf0-06e2-4470-b07b-6fe55959ac36";
        private readonly HttpClient httpClient;

        public AuchanGasStationsFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<AddGasStationModel>> GetAuchanGasStations()
        {
            var stores = await GetAuchanStores();
            var gasStations = new List<AddGasStationModel>();

            foreach (var store in stores)
            {
                if (await HasGasStation(store.store_url_name.pl))
                {
                    var gasStation = ToGasStation(store);
                    gasStations.Add(gasStation);
                }
            }

            return gasStations;
        }

        private Task<List<AuchanStoreListItemResponse>> GetAuchanStores()
        {
            return SendRequest<List<AuchanStoreListItemResponse>>(AuchanStoresUrl);
        }

        private async Task<bool> HasGasStation(string id)
        {
            var store = await SendRequest<AuchanStoreResponse>($"{ AuchanStoresUrl }/{ id }");
            return store?.gasstation?.state == true;
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

        private static AddGasStationModel ToGasStation(AuchanStoreListItemResponse listItem) =>
            new AddGasStationModel
            {
                AddressLine1 = listItem.street_address,
                AddressLine2 = $"{ listItem.zip_code } { listItem.city }",
                Name = listItem.post_title,
                WebsiteAddress = listItem.store_url_name.pl
            };
    }
}
