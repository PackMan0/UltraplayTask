using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbstractionProvider;
using AbstractionProvider.Configurations;
using AbstractionProvider.Interfaces.Repositories;
using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExternalDataService
{
    public class PeriodicSportService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BusinessConfig _businessConfig;
        private readonly IExternalSportService _externalSportService;
        private readonly CacheProvider _cacheProvider;
        private readonly IUpdateSportDataService _updateSportDataService;

        public PeriodicSportService(IOptions<BusinessConfig> settings,
                                    IExternalSportService externalSportService,
                                    CacheProvider cacheProvider,
                                    IUpdateSportDataService updateSportDataService)
        {
            this._externalSportService = externalSportService;
            this._cacheProvider = cacheProvider;
            this._updateSportDataService = updateSportDataService;
            this._cancellationTokenSource = new CancellationTokenSource();
            this._businessConfig = settings.Value;
        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while(!stoppingToken.IsCancellationRequested)
            {

                var newSportData = FilterMatches(await this._externalSportService.GetSportDataAsync());
                var oldSportData = this._cacheProvider.Get<Sport>(this._businessConfig.SportCacheKey);

                try
                {


                    if(oldSportData != null)
                    {

                        await this._updateSportDataService.DeleteSportData(oldSportData, newSportData);
                    }
                    else
                    {
                        /*if(this._repository.GetAll<Sport>().Any(s => s.ExternalID == sportResult.ExternalID) == false)
                        {
                            this._repository.Insert(sportResult);
                        }*/
                    }
                }
                catch(Exception e)
                {
                    Trace.WriteLine(e);
                }
                
                this._cacheProvider.Add(newSportData, this._businessConfig.SportCacheKey);

                await Task.Delay(this._businessConfig.UpdateSportDataIntervalInSeconds * 100, stoppingToken);
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(this._cancellationTokenSource.Token);

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
                this._cancellationTokenSource.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            this._cancellationTokenSource.Cancel();
        }

        private Sport FilterMatches(Sport sportToFilter)
        {
            var matchesIntervalStart = this._businessConfig.GetMatchesIntervalStart;
            var matchesIntervalEnd = matchesIntervalStart.AddSeconds(this._businessConfig.GetEventIntervalInSeconds);
            var sportResult = new Sport()
            {
                ID = sportToFilter.ID,
                ExternalID = sportToFilter.ExternalID,
                Name = sportToFilter.Name
            };

            foreach(var ev in sportToFilter.Events)
            {
                var matches = ev.Matches.Where(m => matchesIntervalEnd >= m.StartDate && m.StartDate >= matchesIntervalStart).ToList();

                if(matches.Any())
                {
                    foreach(var match in matches)
                    {
                        match.EventExtarnalID = ev.ExternalID;

                        foreach(var bet in match.Bets)
                        {
                            bet.MatchExternalID = match.ExternalID;

                            foreach(var odd in bet.Odds)
                            {
                                odd.BetExtarnalID = bet.ExternalID;
                            }
                        }
                    }

                    ev.SportExternaID = sportResult.ExternalID;
                    ev.Matches = matches;

                    sportResult.Events.Add(ev);
                }
            }

            return sportResult;
        }
    }
}
