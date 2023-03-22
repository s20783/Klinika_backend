using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class KlientCreateRequest
    {
        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public string NumerTelefonu { get; set; }

        public string Email { get; set; }

        public string NazwaUzytkownika { get; set; }

        public string Haslo { get; set; }

        public string Haslo2 { get; set; }
    }
}
