using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class WeterynarzSpecjalizacja
    {
        public int IdOsoba { get; set; }
        public int IdSpecjalizacja { get; set; }

        public virtual Weterynarz IdOsobaNavigation { get; set; }
        public virtual Specjalizacja IdSpecjalizacjaNavigation { get; set; }
    }
}
