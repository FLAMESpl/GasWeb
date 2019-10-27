using GasWeb.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GasWeb.Domain.Initialization
{
    internal class SystemUserSeeder
    {
        private readonly GasWebDbContext dbContext;
        private readonly ILogger<SystemUserSeeder> logger;

        public SystemUserSeeder(GasWebDbContext dbContext, ILogger<SystemUserSeeder> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<long> SeedSystemUser()
        {
            var existingSystemUser = await dbContext.Users.SingleOrDefaultAsync(x => x.Role == Shared.Users.UserRole.System);
            if (existingSystemUser != null) return existingSystemUser.Id;

            logger.LogInitializationProcess("Adding system user");

            var systemUser = new User(
                name: "System",
                nameId: "SYSTEM",
                authenticationSchema: AuthenticationSchema.Internal,
                role: Shared.Users.UserRole.System,
                active: true);

            dbContext.Add(systemUser);
            await dbContext.SaveChangesAsync();
            return systemUser.Id;
        }
    }
}
