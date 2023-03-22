using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class LekWMagazynie
    {
        public int IdStanLeku { get; set; }
        public int IdLek { get; set; }
        public int Ilosc { get; set; }
        public DateTime DataWaznosci { get; set; }

        public virtual Lek IdLekNavigation { get; set; }
    }
}
