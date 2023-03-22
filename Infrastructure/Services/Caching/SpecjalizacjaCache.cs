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
    public class SpecjalizacjaCache : ICache<GetSpecjalizacjaResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Specjalizacja";
        public SpecjalizacjaCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetSpecjalizacjaResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetSpecjalizacjaResponse> GetFromCache()
        {
            List<GetSpecjalizacjaResponse> data = memoryChache.Get<List<GetSpecjalizacjaResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}