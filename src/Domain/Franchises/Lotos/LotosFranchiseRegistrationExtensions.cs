using Microsoft.Extensions.DependencyInjection;

namespace GasWeb.Domain.Franchises.Lotos
{
    internal static class LotosFranchiseRegistrationExtensions
    {
        public static void RegisterLotosComponents(this IServiceCollection services)
        {
            services.AddScoped<ILotosWholesalePriceUpdater, LotosWholesalePriceUpdater>();
            services.AddScoped<LotosWholesalePriceFetcher>();
        }
    }
}
