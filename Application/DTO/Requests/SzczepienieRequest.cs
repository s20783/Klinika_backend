using System;

namespace Application.DTO.Requests
{
    public class SzczepienieRequest
    {
        public string IdLek { get; set; }
        public string IdPacjent { get; set; }
        public DateTime Data { get; set; }
        public int Dawka { get; set; }
    }
}