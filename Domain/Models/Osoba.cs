using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Models
{
    public partial class Osoba
    {
        public Osoba()
        {
            Powiadomienies = new HashSet<Powiadomienie>();
        }

        public int IdOsoba { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string NumerTelefonu { get; set; }
        public string Email { get; set; }
        public string NazwaUzytkownika { get; set; }
        public string Haslo { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
        public string Rola { get; set; }
        public DateTime? DataBlokady { get; set; }
        public int LiczbaProb { get; set; }

        public virtual Klient Klient { get; set; }
        public virtual Weterynarz Weterynarz { get; set; }
        public virtual ICollection<Powiadomienie> Powiadomienies { get; set; }
    }
}
