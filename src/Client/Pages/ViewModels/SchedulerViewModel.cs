using GasWeb.Shared.Schedulers;

namespace GasWeb.Client.Pages.ViewModels
{
    public class SchedulerViewModel
    {
        public SchedulerViewModel(Scheduler model)
        {
            Model = model;
        }

        public Scheduler Model { get; }

        public string Interval
        {
            get => Model.Interval.ToString();
            set { }
        }

        public bool Started
        {
            get => Model.StartedAt != null;
            set { }
        }
    }
}
