using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Caching
{
    public class PacjentCache : ICache<GetPacjentListResponse>
    {
        private readonly IMemoryCache memoryChache;
        private readonly string KEY = "Pacjent";
        public PacjentCache(IMemoryCache cache)
        {
            memoryChache = cache;
        }

        public void AddToCache(List<GetPacjentListResponse> data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

            memoryChache.Set(KEY, data, options);
        }

        public List<GetPacjentListResponse> GetFromCache()
        {
            List<GetPacjentListResponse> data = memoryChache.Get<List<GetPacjentListResponse>>(KEY);
            return data;
        }

        public void Remove()
        {
            memoryChache.Remove(KEY);
        }
    }
}