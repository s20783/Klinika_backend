using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Wizytum
    {
        public Wizytum()
        {
            Harmonograms = new HashSet<Harmonogram>();
            WizytaChorobas = new HashSet<WizytaChoroba>();
            WizytaLeks = new HashSet<WizytaLek>();
            WizytaUslugas = new HashSet<WizytaUsluga>();
        }

        public int IdWizyta { get; set; }
        public int IdOsoba { get; set; }
        public int? IdPacjent { get; set; }
        public int? IdZnizka { get; set; }
        public string Opis { get; set; }
        public string NotatkaKlient { get; set; }
        public string Status { get; set; }
        public decimal Cena { get; set; }
        public decimal? CenaZnizka { get; set; }
        public bool CzyOplacona { get; set; }
        public bool CzyZaakceptowanaCena { get; set; }

        public virtual Klient IdOsobaNavigation { get; set; }
        public virtual Pacjent IdPacjentNavigation { get; set; }
        public virtual Znizka IdZnizkaNavigation { get; set; }
        public virtual Receptum Receptum { get; set; }
        public virtual ICollection<Harmonogram> Harmonograms { get; set; }
        public virtual ICollection<WizytaChoroba> WizytaChorobas { get; set; }
        public virtual ICollection<WizytaLek> WizytaLeks { get; set; }
        public virtual ICollection<WizytaUsluga> WizytaUslugas { get; set; }
    }
}
