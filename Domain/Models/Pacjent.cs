using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Pacjent
    {
        public Pacjent()
        {
            Szczepienies = new HashSet<Szczepienie>();
            Wizyta = new HashSet<Wizytum>();
        }

        public int IdPacjent { get; set; }
        public int IdOsoba { get; set; }
        public string Nazwa { get; set; }
        public string Gatunek { get; set; }
        public string Rasa { get; set; }
        public string Masc { get; set; }
        public string Plec { get; set; }
        public DateTime? DataUrodzenia { get; set; }
        public decimal Waga { get; set; }
        public bool Agresywne { get; set; }
        public bool Ubezplodnienie { get; set; }

        public virtual Klient IdOsobaNavigation { get; set; }
        public virtual ICollection<Szczepienie> Szczepienies { get; set; }
        public virtual ICollection<Wizytum> Wizyta { get; set; }
    }
}
