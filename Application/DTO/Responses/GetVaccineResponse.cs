using System;

namespace Application.DTO.Responses
{
    public class GetVaccineResponse
    {
        public string ID_lek { get; set; }
        public string Nazwa { get; set; }
        public string? Producent { get; set; }
        public string Zastosowanie { get; set; }
        public bool CzyObowiazkowa { get; set; }
        public int? OkresWaznosci { get; set; }
    }
}