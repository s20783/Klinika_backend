using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Klient
    {
        public Klient()
        {
            KlientZnizkas = new HashSet<KlientZnizka>();
            Pacjents = new HashSet<Pacjent>();
            Wizyta = new HashSet<Wizytum>();
        }

        public int IdOsoba { get; set; }
        public DateTime DataZalozeniaKonta { get; set; }

        public virtual Osoba IdOsobaNavigation { get; set; }
        public virtual ICollection<KlientZnizka> KlientZnizkas { get; set; }
        public virtual ICollection<Pacjent> Pacjents { get; set; }
        public virtual ICollection<Wizytum> Wizyta { get; set; }
    }
}
