using System;

namespace Application.DTO.Responses
{
    public class GetZnizkaResponse
    {
        public string ID_Znizka { get; set; }
        public string NazwaZnizki { get; set; }
        public decimal ProcentZnizki { get; set; }
    }
}