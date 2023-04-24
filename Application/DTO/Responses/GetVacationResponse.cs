using System;

namespace Application.DTO.Responses
{
    public class GetVacationResponse
    {
        public string IdUrlop { get; set; }
        public string ID_Weterynarz { get; set; }
        public string Weterynarz { get; set; }
        public DateTime Dzien { get; set; }
    }
}