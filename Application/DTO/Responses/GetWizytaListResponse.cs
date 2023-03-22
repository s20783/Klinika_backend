using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetWizytaListResponse
    {
        public string IdWizyta { get; set; }
        public string Status { get; set; }
        public bool CzyOplacona { get; set; }
        public bool CzyZaakceptowanaCena { get; set; }
        public DateTime? Data { get; set; }
        public string IdKlient { get; set; }
        public string Klient { get; set; }
        public string IdWeterynarz { get; set; }
        public string Weterynarz { get; set; }
        public string IdPacjent { get; set; }
        public string Pacjent { get; set; }
    }
}
