using GasWeb.Shared.Schedulers;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Client.WebApiClient
{
    public class SchedulersEndpoint : Endpoint<Scheduler>
    {
        public SchedulersEndpoint(HttpClient httpClient, string route) : base(httpClient, route)
        {
        }

        public Task TriggerManually(long id)
        {
            return httpClient.PostJsonAsync($"{Route}/{id}/trigger", null);
        }
    }
}
