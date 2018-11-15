using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbstractionProvider.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExternalDataService
{
    public class PeriodicSportService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly BusinessConfig _businessConfig;

        public PeriodicSportService(IOptions<BusinessConfig> settings)
        {
            this._businessConfig = settings.Value;
        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while(!stoppingToken.IsCancellationRequested)
            {

                Trace.WriteLine("task runn!!!!!");

                await Task.Delay(this._businessConfig.UpdateSportDataIntervalInSeconds * 1000, stoppingToken);
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if(_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if(_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
