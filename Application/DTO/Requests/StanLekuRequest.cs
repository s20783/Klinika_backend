using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Application.DTO.Request
{
    public partial class StanLekuRequest
    {
        public int Ilosc { get; set; }

        public DateTimeOffset DataWaznosci { get; set; }
    }
}
