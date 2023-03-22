using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Requests
{
    public class ReceptaLekRequest
    {
        public string ID_Recepta { get; set; }
        public string ID_Lek { get; set; }
        public int Ilosc { get; set; }
    }
}
