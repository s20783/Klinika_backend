using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Osoba
    {
        public int IdOsoba { get; set; }
        public int IdRola { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public string NazwaUzytkownika { get; set; }
        public string Haslo { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
        public DateTime? DataBlokady { get; set; }
        public int LiczbaProb { get; set; }

        public virtual Rola IdRolaNavigation { get; set; }
        public virtual Klient Klient { get; set; }
        public virtual Weterynarz Weterynarz { get; set; }
    }
}
