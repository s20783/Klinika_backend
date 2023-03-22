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
    public class KlientCache : ICache<GetKlientListResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Klient";
        public KlientCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetKlientListResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetKlientListResponse> GetFromCache()
        {
            List<GetKlientListResponse> data = memoryChache.Get<List<GetKlientListResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}