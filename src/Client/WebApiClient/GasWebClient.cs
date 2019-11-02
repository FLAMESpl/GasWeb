using GasWeb.Client.Services;
using GasWeb.Shared.Franchises;
using GasWeb.Shared.GasStations;
using GasWeb.Shared.PriceSubmissions;
using GasWeb.Shared.Schedulers;
using GasWeb.Shared.Users;
using System.Net.Http;

namespace GasWeb.Client.WebApiClient
{
    public class GasWebClient
    {
        public GasWebClient(HttpClient httpClient, IAuthService authService)
        {
            Users = new Endpoint<User>(httpClient, "api/users");
            Franchises = new Endpoint<Franchise>(httpClient, "api/franchises");
            GasStations = new Endpoint<GasStation>(httpClient, "api/gas-stations");
            PriceSubmissions = new Endpoint<PriceSubmission>(httpClient, "api/price-submissions");
            Schedulers = new SchedulersEndpoint(httpClient, "api/schedulers");
            AuthService = authService;
        }

        public IAuthService AuthService { get; }
        public Endpoint<User> Users { get; }
        public Endpoint<Franchise> Franchises { get; }
        public Endpoint<GasStation> GasStations { get; }
        public Endpoint<PriceSubmission> PriceSubmissions { get; }
        public SchedulersEndpoint Schedulers { get; }
    }
}
