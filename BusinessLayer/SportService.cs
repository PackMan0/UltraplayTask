using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbstractionProvider;
using AbstractionProvider.Configurations;
using AbstractionProvider.Interfaces.Providers;
using AbstractionProvider.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace BusinessLayer
{
    public class SportService : ISportService
    {
        private readonly BusinessConfig _config;
        private readonly ICacheProvider _cacheProvider;

        public SportService(IOptions<BusinessConfig> config, 
                            ICacheProvider cacheProvider)
        {
            this._cacheProvider = cacheProvider;
            this._config = config.Value;
        }

        public Sport GetAllSportDataAsync()
        {
            var cacheSport = this._cacheProvider.Get<Sport>(this._config.SportCacheKey);
            
            return cacheSport;
        }
    }
}
