using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Receptum
    {
        public Receptum()
        {
            ReceptaLeks = new HashSet<ReceptaLek>();
        }

        public int IdWizyta { get; set; }
        public string Zalecenia { get; set; }

        public virtual Wizytum IdWizytaNavigation { get; set; }
        public virtual ICollection<ReceptaLek> ReceptaLeks { get; set; }
    }
}
