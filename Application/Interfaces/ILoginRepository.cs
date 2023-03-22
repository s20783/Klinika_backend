using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILoginRepository
    {
        public bool CheckCredentails(Osoba user, IPasswordRepository passwordRepository, string haslo, int iterations);
    }
}
