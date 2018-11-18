using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractionProvider;
using AbstractionProvider.Configurations;
using AbstractionProvider.Interfaces.Repositories;
using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExternalDataService
{
    public class UpdateSportDataHub : Hub, IUpdateSportDataService
    {
        private readonly IHubContext<UpdateSportDataHub> _hubContext;
        private readonly CacheProvider _cacheProvider;
        private readonly BusinessConfig _businessConfig;
        private readonly IRepository _repository;

        public UpdateSportDataHub(IHubContext<UpdateSportDataHub> hubContext, CacheProvider cacheProvider, IOptions<BusinessConfig> options, IRepository repository)
        {
            this._hubContext = hubContext;
            this._cacheProvider = cacheProvider;
            _repository = repository;
            this._businessConfig = options.Value;
        }

        public async Task DeleteSportData(Sport oldSporData, Sport newSportData)
        {
            var oldMatchIDs = oldSporData.Events.SelectMany(ev => ev.Matches.Select(m => m.EventExtarnalID));
            var newMatchIDs = newSportData.Events.SelectMany(ev => ev.Matches.Select(m => m.EventExtarnalID));

            var matchIDsToDelete = oldMatchIDs.Where(id => newMatchIDs.Contains(id) == false).ToArray();

            if (matchIDsToDelete.Any())
            {
                await this.SendToAllClients(this._businessConfig.DeleteMatchesMethodName, matchIDsToDelete);
            }
        }

        public async Task AddSportData(Sport oldSporData, Sport newSportData)
        {
            var newEventCollection = newSportData.Events.Where(ev => oldSporData.Events.Contains(ev) == false).ToList();

            if (newEventCollection.Any())
            {
                await this.ProcessNewCollection(newEventCollection,
                                                this._businessConfig.NewEventCollectionCacheKey,
                                                this._businessConfig.NewColectionExperationTimeInSeconds,
                                                this._businessConfig.AddNewEventsMethodName);

                var newMatchCollection = newSportData.Events.Where(newEv => newEventCollection.Contains(newEv) == false)
                                                     .SelectMany(newEv => newEv.Matches)
                                                     .ToList();

                if (newMatchCollection.Any())
                {
                    await this.ProcessNewCollection(newMatchCollection,
                                                    this._businessConfig.NewMatchCollectionCacheKey,
                                                    this._businessConfig.NewColectionExperationTimeInSeconds,
                                                    this._businessConfig.AddNewMatchesMethodName);

                    var newBetCollection = newSportData.Events.Where(newEv => newEventCollection.Contains(newEv) == false)
                                                       .SelectMany(newEv => newEv.Matches)
                                                       .Where(newMatch => newMatchCollection.Contains(newMatch) == false)
                                                       .SelectMany(newMatch => newMatch.Bets)
                                                       .ToList();

                    if (newBetCollection.Any())
                    {
                        await this.ProcessNewCollection(newBetCollection,
                                                        this._businessConfig.NewBetCollectionCacheKey,
                                                        this._businessConfig.NewColectionExperationTimeInSeconds,
                                                        this._businessConfig.AddNewBetsMethodName);

                        var newOddCollection = newSportData.Events.Where(newEv => newEventCollection.Contains(newEv) == false)
                                                           .SelectMany(newEv => newEv.Matches)
                                                           .Where(newMatch => newMatchCollection.Contains(newMatch) == false)
                                                           .SelectMany(newMatch => newMatch.Bets)
                                                           .Where(newBet => newBetCollection.Contains(newBet) == false)
                                                           .SelectMany(newBet => newBet.Odds)
                                                           .ToList();

                        if (newOddCollection.Any())
                        {
                            await this.ProcessNewCollection(newOddCollection,
                                                            this._businessConfig.NewOddCollectionCacheKey,
                                                            this._businessConfig.NewColectionExperationTimeInSeconds,
                                                            this._businessConfig.AddNewOddsMethodName);
                        }
                    }
                }
            }
        }

        private async Task ProcessNewCollection(IEnumerable<Base> newCollection,
                                                string cacheKey,
                                                int experationInterval,
                                                string methodName)
        {
            this._cacheProvider.Add(newCollection,
                                    cacheKey,
                                    experationInterval);

            this._repository.InsertCollection(newCollection);

            await this.SendToAllClients(methodName, cacheKey);
        }

        private async Task SendToAllClients(string MethodName, object objToSend)
        {
            if (this._hubContext?.Clients != null && objToSend!= null)
            {
                await this._hubContext.Clients.All.SendAsync(MethodName, objToSend);
            }
        }
    }
}
