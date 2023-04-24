using System;

namespace Application.DTO.Requests
{
    public class VacationRequest
    {
        public string ID_weterynarz { get; set; }
        public DateTimeOffset Dzien { get; set; }
    }
}