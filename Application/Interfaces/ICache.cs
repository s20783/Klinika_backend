using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICache<T>
    {
        void AddToCache(List<T> data);

        List<T> GetFromCache();

        void Remove();
    }
}