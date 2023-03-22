using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests
{
    public class KlientCreateKlinikaRequest
    {
        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public string NumerTelefonu { get; set; }

        public string Email { get; set; }
    }
}
