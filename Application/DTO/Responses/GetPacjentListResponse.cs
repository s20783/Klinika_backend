using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetPacjentListResponse
    {
        public string IdPacjent { get; set; }
        public string IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Plec { get; set; }
        public string Wlasciciel { get; set; }
    }
}
