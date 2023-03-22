using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Lek
    {
        public Lek()
        {
            ChorobaLeks = new HashSet<ChorobaLek>();
            LekWMagazynies = new HashSet<LekWMagazynie>();
            ReceptaLeks = new HashSet<ReceptaLek>();
            WizytaLeks = new HashSet<WizytaLek>();
        }

        public int IdLek { get; set; }
        public string Nazwa { get; set; }
        public string JednostkaMiary { get; set; }
        public string Producent { get; set; }

        public virtual Szczepionka Szczepionka { get; set; }
        public virtual ICollection<ChorobaLek> ChorobaLeks { get; set; }
        public virtual ICollection<LekWMagazynie> LekWMagazynies { get; set; }
        public virtual ICollection<ReceptaLek> ReceptaLeks { get; set; }
        public virtual ICollection<WizytaLek> WizytaLeks { get; set; }
    }
}
