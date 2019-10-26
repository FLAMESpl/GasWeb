using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GasWeb.Domain.Initialization
{
    public interface ISystemInitializer
    {
        Task InitialzieAsync();
    }

    internal class SystemInitializer : ISystemInitializer
    {
        private readonly SystemUserSeeder systemUserSeeder;
        private readonly FranchiseSeeder franchiseSeeder;
        private readonly ILogger<SystemInitializer> logger;

        public SystemInitializer(
            SystemUserSeeder systemUserSeeder,
            FranchiseSeeder franchiseSeeder,
            ILogger<SystemInitializer> logger)
        {
            this.systemUserSeeder = systemUserSeeder;
            this.franchiseSeeder = franchiseSeeder;
            this.logger = logger;
        }

        public async Task InitialzieAsync()
        {
            logger.LogInitializationProcess("Start");

            var systemUserId = await systemUserSeeder.SeedSystemUser();
            await franchiseSeeder.SeedKnownFranchises(new SystemUserMetadataProvider(systemUserId));

            logger.LogInitializationProcess("End");
        }
    }
}
