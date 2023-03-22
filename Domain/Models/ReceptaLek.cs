using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class ReceptaLek
    {
        public int IdLek { get; set; }
        public int IdWizyta { get; set; }
        public int Ilosc { get; set; }

        public virtual Lek IdLekNavigation { get; set; }
        public virtual Receptum IdWizytaNavigation { get; set; }
    }
}
