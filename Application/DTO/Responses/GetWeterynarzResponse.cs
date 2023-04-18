using System;

namespace Application.DTO.Responses
{
    public class GetWeterynarzResponse
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public DateTime DataZatrudnienia { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public decimal Pensja { get; set; }
    }
}
