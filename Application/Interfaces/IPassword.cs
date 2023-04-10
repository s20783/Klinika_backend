using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPassword
    {
        public string HashPassword(byte[] salt, string plainPassword, int iterations);
        public byte[] GenerateSalt();
        public string GetRandomPassword(int l);
    }
}
