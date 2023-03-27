using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class KlientZnizka
    {
        public int IdOsoba { get; set; }
        public int IdZnizka { get; set; }
        public DateTime DataPrzyznania { get; set; }
        public bool CzyWykorzystana { get; set; }

        public virtual Klient IdOsobaNavigation { get; set; }
        public virtual Znizka IdZnizkaNavigation { get; set; }
    }
}
