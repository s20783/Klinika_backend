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
    public class WeterynarzCache : ICache<GetWeterynarzListResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Weterynarz";
        public WeterynarzCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetWeterynarzListResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetWeterynarzListResponse> GetFromCache()
        {
            List<GetWeterynarzListResponse> data = memoryChache.Get<List<GetWeterynarzListResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}