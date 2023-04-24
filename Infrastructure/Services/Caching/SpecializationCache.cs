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
    public class SpecializationCache : ICache<GetSpecializationResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Specjalizacja";
        public SpecializationCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetSpecializationResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetSpecializationResponse> GetFromCache()
        {
            List<GetSpecializationResponse> data = memoryChache.Get<List<GetSpecializationResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}