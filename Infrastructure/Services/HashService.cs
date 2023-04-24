using Application.Interfaces;
using HashidsNet;
using System;

namespace Infrastructure.Services
{
    public class HashService : IHash
    {
        private readonly IHashids _hashids;
        public HashService(IHashids hashids)
        {
            _hashids = hashids;
        }

        public int Decode(string value)
        {
            var idArray = _hashids.Decode(value);
            if (idArray.Length == 0)
            {
                throw new Exception();
            }
            int id = idArray[0];

            return id;
        }

        public (int, int) Decode(string value1, string value2)
        {
            var idArray1 = _hashids.Decode(value1);
            var idArray2 = _hashids.Decode(value2);
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
            return _hashids.Encode(value);
        }
    }
}
