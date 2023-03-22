using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetUslugaPacjentResponse
    {
        public string ID_wizyta { get; set; }
        public string ID_Usluga { get; set; }
        public string NazwaUslugi { get; set; }
        public string Opis { get; set; }
        public DateTime Data { get; set; }
    }
}
