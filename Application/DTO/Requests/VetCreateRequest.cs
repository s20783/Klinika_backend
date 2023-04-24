using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class VetCreateRequest
    {
        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public DateTimeOffset DataUrodzenia { get; set; }

        public string NumerTelefonu { get; set; }

        public string Email { get; set; }

        public decimal Pensja { get; set; }

        public DateTimeOffset DataZatrudnienia { get; set; }
    }
}
