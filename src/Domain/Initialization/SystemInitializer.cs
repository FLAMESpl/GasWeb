using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GasWeb.Domain.Initialization
{
    public interface ISystemInitializer
    {
        Task<long> InitalizeSystemUserAsync();
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

        public Task<long> InitalizeSystemUserAsync()
        {
            return systemUserSeeder.SeedSystemUser();
        }

        public async Task InitialzieAsync()
        {
            logger.LogInitializationProcess("Start");

            await franchiseSeeder.SeedKnownFranchises();
            await schedulerSeeder.SeedSchedulers();

            logger.LogInitializationProcess("End");
        }
    }
}
