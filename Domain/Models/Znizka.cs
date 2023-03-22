using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Znizka
    {
        public Znizka()
        {
            KlientZnizkas = new HashSet<KlientZnizka>();
            Wizyta = new HashSet<Wizytum>();
        }

        public int IdZnizka { get; set; }
        public string NazwaZnizki { get; set; }
        public decimal ProcentZnizki { get; set; }
        public DateTime? DoKiedy { get; set; }

        public virtual ICollection<KlientZnizka> KlientZnizkas { get; set; }
        public virtual ICollection<Wizytum> Wizyta { get; set; }
    }
}
