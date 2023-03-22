using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Harmonogram
    {
        public int IdHarmonogram { get; set; }
        public int WeterynarzIdOsoba { get; set; }
        public int? IdWizyta { get; set; }
        public DateTime DataRozpoczecia { get; set; }
        public DateTime DataZakonczenia { get; set; }

        public virtual Wizytum IdWizytaNavigation { get; set; }
        public virtual Weterynarz WeterynarzIdOsobaNavigation { get; set; }
    }
}
