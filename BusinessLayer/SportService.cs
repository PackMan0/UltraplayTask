using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbstractionProvider;
using AbstractionProvider.Configurations;
using AbstractionProvider.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace BusinessLayer
{
    public class SportService : ISportService
    {
        private readonly IExternalSportService _externalSportService;
        private readonly BusinessConfig _config;
        private readonly CacheProvider _cacheProvider;
        private readonly IRepository _repository;

        public SportService(IExternalSportService externalSportService, 
                            IOptions<BusinessConfig> config, 
                            CacheProvider cacheProvider,
                            IRepository repository)
        {
            this._externalSportService = externalSportService;
            this._cacheProvider = cacheProvider;
            this._repository = repository;
            this._config = config.Value;
        }

        public async Task<Sport> GetAllSportDataAsync()
        {
            var sportResult = FilterMatches(await this._externalSportService.GetSportDataAsync());
            var cacheSport = this._cacheProvider.Get<Sport>(this._config.SportCacheKey);

            if (cacheSport == null)
            {
                if (this._repository.GetAll<Sport>().Any(s => s.ExternalID == sportResult.ExternalID) == false)
                {
                    this._repository.Insert(sportResult);
                }
            }

            this._cacheProvider.Add(sportResult, this._config.SportCacheKey);
            
            return sportResult;
        }

        private Sport FilterMatches(Sport sportToFilter)
        {
            var matchesIntervalStart = this._config.GetMatchesIntervalStart;
            var matchesIntervalEnd = matchesIntervalStart.AddSeconds(this._config.GetEventIntervalInSeconds);
            var sportResult = new Sport()
                              {
                                                  ID = sportToFilter.ID,
                                                  ExternalID = sportToFilter.ExternalID,
                                                  Name = sportToFilter.Name
                              };

            foreach (var ev in sportToFilter.Events)
            {
                var matches = ev.Matches.Where(m => matchesIntervalEnd >= m.StartDate && m.StartDate >= matchesIntervalStart).ToList();

                if (matches.Any())
                {
                    foreach (var match in matches)
                    {
                        match.EventExtarnalID = ev.ExternalID;

                        foreach (var bet in match.Bets)
                        {
                            bet.MatchExternalID = match.ExternalID;

                            foreach (var odd in bet.Odds)
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
