using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Application.DTO.Request
{
    public class PatientRequest
    {
        public string IdOsoba { get; set; }

        public string Nazwa { get; set; }

        public string Gatunek { get; set; }

        public string Rasa { get; set; }

        public string Masc { get; set; }

        public string Plec { get; set; }

        public DateTimeOffset DataUrodzenia { get; set; }

        public decimal Waga { get; set; }

        public bool Agresywne { get; set; }

        public bool Ubezplodnienie { get; set; }
    }
}
