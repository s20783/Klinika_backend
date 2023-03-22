using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class LoginRequest
    {
        public string NazwaUzytkownika { get; set; }

        public string Haslo { get; set; }
    }
}
