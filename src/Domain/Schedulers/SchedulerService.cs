using GasWeb.Shared;
using GasWeb.Shared.Schedulers;
using GasWeb.Shared.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Domain.Schedulers
{
    public interface ISchedulerService
    {
        Task Update(long id, UpdateSchedulerModel model);
        Task<Scheduler> Get(long id);
        Task<PageResponse<Scheduler>> GetList(int pageNumber, int pageSize);
        Task Trigger(long id);
    }

    internal class SchedulerService : ISchedulerService
    {
        private readonly GasWebDbContext dbContext;
        private readonly IAuditMetadataProvider auditMetadataProvider;
        private readonly SchedulerTaskDispatcher taskDispatcher;
        private readonly UserContext userContext;

        public SchedulerService(
            GasWebDbContext dbContext,
            IAuditMetadataProvider auditMetadataProvider,
            SchedulerTaskDispatcher taskDispatcher,
            UserContext userContext)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
            this.taskDispatcher = taskDispatcher;
            this.userContext = userContext;
        }

        public async Task<Scheduler> Get(long id)
        {
            var scheduler = await dbContext.Schedulers.GetAsync(id);
            return scheduler?.ToContract();
        }

        public Task<PageResponse<Scheduler>> GetList(int pageNumber, int pageSize)
        {
            return dbContext.Schedulers.PickPageAsync(pageNumber, pageSize, x => x.ToContract());
        }

        public async Task Update(long id, UpdateSchedulerModel model)
        {
            var scheduler = await dbContext.Schedulers.GetAsync(id);
            scheduler.Update(model);
            auditMetadataProvider.UpdateAuditMetadataInExistingEntiy(scheduler);
            await dbContext.SaveChangesAsync();
        }

        public async Task Trigger(long id)
        {
            userContext.Id = await dbContext.Users.Where(x => x.Role == UserRole.System).Select(x => x.Id).SingleAsync();
            var scheduler = await dbContext.Schedulers.GetAsync(id);
            await taskDispatcher.ExecuteTask(id);
            scheduler.Run();
            await dbContext.SaveChangesAsync();
        }
    }
}
