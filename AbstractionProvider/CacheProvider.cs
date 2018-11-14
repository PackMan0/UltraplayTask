using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AbstractionProvider.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AbstractionProvider
{
    public class CacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public CacheProvider(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        public void Add<T>(T objInfo, string key, int experationTimeInSeconds)
        {
            this._memoryCache.Set(key,
                                  objInfo,
                                  TimeSpan.FromSeconds(experationTimeInSeconds));
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
