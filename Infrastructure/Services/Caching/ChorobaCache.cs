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
    public class ChorobaCache : ICache<GetChorobaResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Choroba";
        public ChorobaCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetChorobaResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetChorobaResponse> GetFromCache()
        {
            List<GetChorobaResponse> data = memoryChache.Get<List<GetChorobaResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}