using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetSzczepienieResponse
    {
        public string IdSzczepienie { get; set; }
        public string IdLek { get; set; }
        public string IdPacjent { get; set; }
        public string Nazwa { get; set; }
        public DateTime Data { get; set; }
        public DateTime? DataWaznosci { get; set; }
        public string Zastosowanie { get; set; }
        public int Dawka { get; set; }
    }
}