using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Application.DTO.Request
{
    public partial class MedicamentWarehouseRequest
    {
        public int Ilosc { get; set; }

        public DateTimeOffset DataWaznosci { get; set; }
    }
}
