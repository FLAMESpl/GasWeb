using System;
using GasWeb.Shared.Schedulers;

namespace GasWeb.Domain.Schedulers.Entities
{
    internal class Scheduler : AuditEntity
    {
        public Scheduler(long franchiseId, TimeSpan interval, DateTime? startedAt)
        {
            FranchiseId = franchiseId;
            Interval = interval;
            StartedAt = startedAt;
        }

        public long FranchiseId { get; private set; }
        public TimeSpan Interval { get; private set; }
        public DateTime? StartedAt { get; private set; }

        internal void Update(UpdateSchedulerModel model)
        {
            Interval = model.Interval;

            if (model.Running)
            {
                if (StartedAt == null)
                    StartedAt = DateTime.UtcNow;
            }
            else
            {
                StartedAt = null;
            }
        }
    }
}
