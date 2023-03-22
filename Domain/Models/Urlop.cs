using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Urlop
    {
        public int IdUrlop { get; set; }
        public int IdOsoba { get; set; }
        public DateTime Dzien { get; set; }

        public virtual Weterynarz IdOsobaNavigation { get; set; }
    }
}
