using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Usluga
    {
        public Usluga()
        {
            WizytaUslugas = new HashSet<WizytaUsluga>();
        }

        public int IdUsluga { get; set; }
        public string NazwaUslugi { get; set; }
        public string Opis { get; set; }
        public decimal Cena { get; set; }
        public bool Narkoza { get; set; }
        public string Dolegliwosc { get; set; }

        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
    }
}
