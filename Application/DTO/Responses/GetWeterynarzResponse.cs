﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetWeterynarzResponse
    {
        public int IdOsoba { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public DateTime DataZatrudnienia { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public decimal Pensja { get; set; }
    }
}
