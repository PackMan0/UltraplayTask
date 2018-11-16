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

        public Sport GetAllSportDataAsync()
        {
            var cacheSport = this._cacheProvider.Get<Sport>(this._config.SportCacheKey);
            
            return cacheSport;
        }
    }
}
