using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Szczepionka
    {
        public Szczepionka()
        {
            Szczepienies = new HashSet<Szczepienie>();
        }

        public int IdLek { get; set; }
        public string Zastosowanie { get; set; }
        public bool CzyObowiazkowa { get; set; }
        public long? OkresWaznosci { get; set; }

        public virtual Lek IdLekNavigation { get; set; }
        public virtual ICollection<Szczepienie> Szczepienies { get; set; }
    }
}
