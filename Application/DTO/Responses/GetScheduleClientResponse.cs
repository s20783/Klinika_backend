using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetScheduleClientResponse
    {
        public string IdHarmonogram { get; set; }
        public string IdWeterynarz { get; set; }
        public string Weterynarz { get; set; }
        public DateTime Data { get; set; }
        public int Dzien { get; set; }
    }
}
