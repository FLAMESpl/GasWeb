using GasWeb.Shared.Schedulers;

namespace GasWeb.Domain.Schedulers
{
    internal static class TypeMaps
    {
        internal static Scheduler ToContract(this Entities.Scheduler scheduler) =>
            new Scheduler
            {
                FranchiseId = scheduler.FranchiseId,
                Id = scheduler.Id,
                Interval = scheduler.Interval,
                StartedAt = scheduler.StartedAt,
                LastModifiedAt = scheduler.LastModified,
                LastModifiedByUserId = scheduler.ModifiedByUserId
            };
    }
}
