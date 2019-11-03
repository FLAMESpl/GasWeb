using GasWeb.Shared.PriceSubmissions;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Client.WebApiClient
{
    public class PriceSubmissionsEndpoint : Endpoint<PriceSubmission>
    {
        public PriceSubmissionsEndpoint(HttpClient httpClient, string route) : base(httpClient, route)
        {
        }

        public Task Rate(long id, AddPriceSubmissionRatingModel model)
        {
            return httpClient.SendJsonAsync(HttpMethod.Put, $"{Route}/{id}/rate", model);
        }
    }
}
