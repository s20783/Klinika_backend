using System;

namespace Application.DTO.Responses
{
    public class GetPacjentDetailsResponse
    {
        public string IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Masc { get; set; }
        public string Plec { get; set; }
        public DateTime? DataUrodzenia { get; set; }
        public decimal Waga { get; set; }
        public bool Agresywne { get; set; }
        public string Wlasciciel { get; set; }
        public bool Ubezplodnienie { get; set; }
    }
}
