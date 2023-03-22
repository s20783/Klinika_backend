using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IHash
    {
        public string Encode(int value);
        public int Decode(string value);
        public (int,int) Decode(string value1, string value2);
    }
}
