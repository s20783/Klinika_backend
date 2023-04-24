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
    public class ServiceCache : ICache<GetServiceResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Usluga";
        public ServiceCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetServiceResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetServiceResponse> GetFromCache()
        {
            List<GetServiceResponse> data = memoryChache.Get<List<GetServiceResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}