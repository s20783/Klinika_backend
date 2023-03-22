using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Responses
{
    public class GetLekResponse
    {
        public string IdLek { get; set; }
        public string Nazwa { get; set; }
        public string JednostkaMiary { get; set; }
        public string Producent { get; set; }
    }
}