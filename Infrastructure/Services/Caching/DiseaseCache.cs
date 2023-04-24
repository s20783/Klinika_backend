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
    public class DiseaseCache : ICache<GetDiseaseResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Choroba";
        public DiseaseCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetDiseaseResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(90)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetDiseaseResponse> GetFromCache()
        {
            List<GetDiseaseResponse> data = memoryChache.Get<List<GetDiseaseResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}