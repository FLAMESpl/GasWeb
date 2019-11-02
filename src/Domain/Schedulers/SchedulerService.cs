using GasWeb.Shared;
using GasWeb.Shared.Schedulers;
using System;
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

        public SchedulerService(
            GasWebDbContext dbContext,
            IAuditMetadataProvider auditMetadataProvider,
            SchedulerTaskDispatcher taskDispatcher)
        {
            this.dbContext = dbContext;
            this.auditMetadataProvider = auditMetadataProvider;
            this.taskDispatcher = taskDispatcher;
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
            var scheduler = await dbContext.Schedulers.GetAsync(id);
            await taskDispatcher.ExecuteTask(id);
            scheduler.Run();
            await dbContext.SaveChangesAsync();
        }
    }
}
