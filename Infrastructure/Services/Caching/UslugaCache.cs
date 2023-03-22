using Application.DTO.Responses;
using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Caching
{
    public class UslugaCache : ICache<GetUslugaResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Usluga";
        public UslugaCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetUslugaResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetUslugaResponse> GetFromCache()
        {
            List<GetUslugaResponse> data = memoryChache.Get<List<GetUslugaResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}