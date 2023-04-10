using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILogin
    {
        public bool CheckCredentails(Osoba user, IPassword passwordRepository, string haslo, int iterations);
    }
}
