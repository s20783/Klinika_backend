using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Specjalizacja
    {
        public Specjalizacja()
        {
            WeterynarzSpecjalizacjas = new HashSet<WeterynarzSpecjalizacja>();
        }

        public int IdSpecjalizacja { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
    }
}
