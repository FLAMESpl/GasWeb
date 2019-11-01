using System;

namespace GasWeb.Shared.Schedulers
{
    public class Scheduler
    {
        public long Id { get; set; }
        public long FranchiseId { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public long LastModifiedByUserId { get; set; }
    }
}
