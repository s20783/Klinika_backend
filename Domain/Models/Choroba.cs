using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Choroba
    {
        public Choroba()
        {
            ChorobaLeks = new HashSet<ChorobaLek>();
            WizytaChorobas = new HashSet<WizytaChoroba>();
        }

        public int IdChoroba { get; set; }
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public string NazwaLacinska { get; set; }

        public virtual ICollection<ChorobaLek> ChorobaLeks { get; set; }
        public virtual ICollection<WizytaChoroba> WizytaChorobas { get; set; }
    }
}
