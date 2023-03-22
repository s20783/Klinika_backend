using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetKlientZnizkaResponse
    {
        public string ID_Znizka { get; set; }
        public string NazwaZnizki { get; set; }
        public decimal ProcentZnizki { get; set; }
        public bool CzyWykorzystana { get; set; }
    }
}