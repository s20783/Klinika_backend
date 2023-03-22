using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Powiadomienie
    {
        public int IdPowiadomienie { get; set; }
        public int IdOsoba { get; set; }
        public string Tekst { get; set; }
        public DateTime Data { get; set; }
        public bool CzyWyswietlona { get; set; }
        public string Kategoria { get; set; }

        public virtual Osoba IdOsobaNavigation { get; set; }
    }
}
