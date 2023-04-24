using System;

namespace Application.DTO.Requests
{
    public class VaccineRequest
    {
        public string Nazwa { get; set; }
        public string? Producent { get; set; }
        public string Zastosowanie { get; set; }
        public bool CzyObowiazkowa { get; set; }
        public int? OkresWaznosci { get; set; }
    }
}