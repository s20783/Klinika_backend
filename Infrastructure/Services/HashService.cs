using Application.Interfaces;
using HashidsNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HashService : IHash
    {
        private readonly IHashids hashids;
        public HashService(IHashids _hashids)
        {
            hashids = _hashids;
        }

        public int Decode(string value)
        {
            var idArray = hashids.Decode(value);
            if (idArray.Length == 0)
            {
                throw new Exception();
            }
            int id = idArray[0];

            return id;
        }

        public (int, int) Decode(string value1, string value2)
        {
            var idArray1 = hashids.Decode(value1);
            var idArray2 = hashids.Decode(value2);
            if (idArray1.Length == 0 || idArray2.Length == 0)
            {
                throw new Exception();
            }
            int id1 = idArray1[0];
            int id2 = idArray2[0];

            return (id1, id2);
        }

        public string Encode(int value)
        {
            return hashids.Encode(value);
        }
    }
}
