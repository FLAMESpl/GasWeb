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
        private readonly SchedulerSeeder schedulerSeeder;
        private readonly ILogger<SystemInitializer> logger;

        public SystemInitializer(
            SystemUserSeeder systemUserSeeder,
            FranchiseSeeder franchiseSeeder,
            SchedulerSeeder schedulerSeeder,
            ILogger<SystemInitializer> logger)
        {
            this.systemUserSeeder = systemUserSeeder;
            this.franchiseSeeder = franchiseSeeder;
            this.schedulerSeeder = schedulerSeeder;
            this.logger = logger;
        }

        public async Task InitialzieAsync()
        {
            logger.LogInitializationProcess("Start");

            var systemUserId = await systemUserSeeder.SeedSystemUser();
            var metadataProvider = new SystemUserMetadataProvider(systemUserId);
            await franchiseSeeder.SeedKnownFranchises(metadataProvider);
            await schedulerSeeder.SeedSchedulers(metadataProvider);

            logger.LogInitializationProcess("End");
        }
    }
}
