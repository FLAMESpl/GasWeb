using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GasWeb.Domain.Schedulers
{
    public interface IScheduledExecutor
    {
        Task Run(CancellationToken cancellationToken);
    }

    internal class ScheduledExecutor : IScheduledExecutor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<ScheduledExecutor> logger;

        public ScheduledExecutor(
            IServiceScopeFactory scopeFactory,
            ILogger<ScheduledExecutor> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            var i = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (i == 5)
                {
                    try
                    {
                        i = 0;
                        await CheckTasks();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                    }
                }

                i++;
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private async Task CheckTasks()
        {
            logger.LogDebug("Checking for scheduled tasks.");

            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GasWebDbContext>();
            var schedulerService = scope.ServiceProvider.GetRequiredService<ISchedulerService>();
            var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
            var systemUser = await dbContext.Users.Where(x => x.Role == Shared.Users.UserRole.System).SingleAsync();
            var schedulers = await dbContext.Schedulers.Where(x => x.StartedAt != null).ToListAsync();
            var now = DateTime.UtcNow;

            userContext.Id = systemUser.Id;

            foreach (var scheduler in schedulers)
            {
                if (scheduler.LastRun == null || (now - scheduler.LastRun.Value) >= scheduler.Interval)
                {
                    logger.LogDebug($"Running task {scheduler.Id}.");

                    try
                    {
                        await schedulerService.Trigger(scheduler.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                    }
                }
            }
        }
    }
}
