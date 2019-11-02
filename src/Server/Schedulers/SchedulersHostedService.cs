using GasWeb.Domain.Schedulers;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GasWeb.Server.Schedulers
{
    public class SchedulersHostedService : IHostedService
    {
        private readonly IScheduledExecutor scheduledExecutor;
        private Thread thread;

        public SchedulersHostedService(IScheduledExecutor scheduledExecutor)
        {
            this.scheduledExecutor = scheduledExecutor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            thread = new Thread(new ThreadStart(() => scheduledExecutor.Run(cancellationToken).Wait()));
            thread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            thread.Join(TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }
    }
}
