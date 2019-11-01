using System;

namespace GasWeb.Shared.Schedulers
{
    public class UpdateSchedulerModel
    {
        public TimeSpan Interval { get; set; }
        public bool Running { get; set; }
    }
}
