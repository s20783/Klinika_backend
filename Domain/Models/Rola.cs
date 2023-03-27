using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Rola
    {
        public Rola()
        {
            Osobas = new HashSet<Osoba>();
        }

        public int IdRola { get; set; }
        public string Nazwa { get; set; }

        public virtual ICollection<Osoba> Osobas { get; set; }
    }
}
