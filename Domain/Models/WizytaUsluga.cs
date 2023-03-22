using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class WizytaUsluga
    {
        public int IdWizyta { get; set; }
        public int IdUsluga { get; set; }

        public virtual Usluga IdUslugaNavigation { get; set; }
        public virtual Wizytum IdWizytaNavigation { get; set; }
    }
}
