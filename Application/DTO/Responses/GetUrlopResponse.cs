using System;

namespace Application.DTO.Responses
{
    public class GetUrlopResponse
    {
        public string IdUrlop { get; set; }
        public string ID_Weterynarz { get; set; }
        public string Weterynarz { get; set; }
        public DateTime Dzien { get; set; }
    }
}