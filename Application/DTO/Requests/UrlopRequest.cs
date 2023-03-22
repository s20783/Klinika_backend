using System;

namespace Application.DTO.Requests
{
    public class UrlopRequest
    {
        public string ID_weterynarz { get; set; }
        public DateTimeOffset Dzien { get; set; }
    }
}