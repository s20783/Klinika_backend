using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MemoryMockCache<T> : ICache<T>
    {
        public void AddToCache(List<T> data)
        {
            //add to cache
        }

        public List<T> GetFromCache()
        {
            return null;
        }

        public void Remove()
        {
            //remove from cache
        }
    }
}