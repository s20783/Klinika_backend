using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetPacjentKlientListResponse
    {
        public string IdPacjent { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Plec { get; set; }
        public bool Agresywne { get; set; }

    }
}
