using System;

namespace Application.DTO.Responses
{
    public class GetWizytaDetailsKlientResponse
    {
        public string Status { get; set; }
        public bool CzyOplacona { get; set; }
        public DateTime? DataRozpoczecia { get; set; }
        public DateTime? DataZakonczenia { get; set; }
        public string Opis { get; set; }
        public string NotatkaKlient { get; set; }
        public decimal? Cena { get; set; }
        public string Weterynarz { get; set; }
        public string IdPacjent { get; set; }
        public string Pacjent { get; set; }
    }
}
