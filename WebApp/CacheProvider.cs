﻿using System;
using AbstractionProvider.Interfaces.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace WebApp
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public CacheProvider(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        public void Add<T>(T objInfo, string key, int experationTimeInSeconds = 0)
        {
            if (experationTimeInSeconds == 0)
            {
                this._memoryCache.Set(key,
                                      objInfo,
                                      new MemoryCacheEntryOptions(){Priority = CacheItemPriority.NeverRemove });
            }
            else
            {
                this._memoryCache.Set(key,
                                      objInfo,
                                      TimeSpan.FromSeconds(experationTimeInSeconds));
            }
        }
        public void Clear(string key)
        {
            this._memoryCache.Remove(key);
        }
        
        public T Get<T>(string key) where T : class
        {
            T value = null;
            
            if (this._memoryCache.TryGetValue(key, out value))
            {
                return value;
            }

            return null;
        }
    }
}
