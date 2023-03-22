using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Test
{
    public class MockData
    {
        public static List<Osoba> GetOsobaList()
        {
            return new List<Osoba>
            {
                new Osoba       //klient
                {
                    IdOsoba = 1,
                    Imie = "aaa",
                    Nazwisko = "bbb",
                    NazwaUzytkownika = "Adam1",
                    Email = "Adam1@gmail.com",
                    NumerTelefonu = "123456789",
                    Salt = "ayH570KmANkGyHqQroN6Nl30mclzaC6Rxfq4SedA+C4=",
                    Haslo = "TP506vFmQn79Wumsfl012OL3XCvaDsnKGBsjZbRYrZdjnZOrtdaKpyK9VxDN5/+faDZwWUuT2xLbDv0gegrWAg==",
                    Rola = ""
                },
                new Osoba       //weterynarz
                {
                    IdOsoba = 2,
                    Imie = "aaa",
                    Nazwisko = "bbb",
                    NazwaUzytkownika = "Osoba2",
                    Email = "osoba2@gmail.com",
                    NumerTelefonu = "123456789",
                },
                new Osoba       //admin
                {
                    IdOsoba = 3,
                    Imie = "aaa",
                    Nazwisko = "bbb",
                    NazwaUzytkownika = "Osoba2",
                    Email = "osoba2@gmail.com",
                    NumerTelefonu = "123456789",
                }
            };
        }


        public static List<Klient> GetKlientList()
        {
            return new List<Klient>
            {
                new Klient
                {
                    IdOsoba = 1,
                    DataZalozeniaKonta = DateTime.Now
                }
            };
        }

        public static List<Pacjent> GetPacjentList()
        {
            return new List<Pacjent>
            {
                new Pacjent
                {
                    IdPacjent = 1,
                    DataUrodzenia = DateTime.Now.AddDays(-1),
                    Ubezplodnienie = false,
                    Gatunek = "Kot",
                    Waga = 4
                }
            };
        }


        public static List<Weterynarz> GetWeterynarzList()
        {
            return new List<Weterynarz>
            {
                new Weterynarz
                {
                    IdOsoba = 2,
                    DataZatrudnienia = DateTime.Now.AddDays(-5),
                    DataUrodzenia = DateTime.Now.AddDays(-10),
                    Pensja = 5000
                }
            };
        }


        public static List<GodzinyPracy> GetGodzinyPracyList()
        {
            return new List<GodzinyPracy>
            {
                new GodzinyPracy
                {
                    IdOsoba = 2,
                    DzienTygodnia = 1,
                    GodzinaRozpoczecia = new TimeSpan(10,0,0),
                    GodzinaZakonczenia = new TimeSpan(13,0,0)
                },
                new GodzinyPracy
                {
                    IdOsoba = 2,
                    DzienTygodnia = 2,
                    GodzinaRozpoczecia = new TimeSpan(10,0,0),
                    GodzinaZakonczenia = new TimeSpan(13,17,0)
                },
                new GodzinyPracy
                {
                    IdOsoba = 2,
                    DzienTygodnia = 3,
                    GodzinaRozpoczecia = new TimeSpan(10,0,0),
                    GodzinaZakonczenia = new TimeSpan(13,0,0)
                }
            };
        }


        public static List<Specjalizacja> GetSpecjalizacjaList()
        {
            return new List<Specjalizacja>
            {
                new Specjalizacja
                {
                    IdSpecjalizacja = 1,
                    Opis = "aaaaaaaaaaaa",
                    Nazwa = "bbbbbbbbbbbb"
                },
                new Specjalizacja
                {
                    IdSpecjalizacja = 2,
                    Opis = "aaaaaaaaaaaa",
                    Nazwa = "bbbbbbbbbbbb"
                }
            };
        }


        public static List<WeterynarzSpecjalizacja> GetWeterynarzSpecjalizacjaList()
        {
            return new List<WeterynarzSpecjalizacja>
            {
                new WeterynarzSpecjalizacja
                {
                    IdSpecjalizacja = 1,
                    IdOsoba = 2
                }
            };
        }


        public static List<Lek> GetLekList()
        {
            return new List<Lek>
            {
                new Lek
                {
                    IdLek = 1,
                    Nazwa = "LekNazwa",
                    JednostkaMiary = "ml"
                },
                new Lek
                {
                    IdLek = 2,
                    Nazwa = "Szczepionka 1",
                    JednostkaMiary = "ml"
                }
            };
        }


        public static List<LekWMagazynie> GetLekWMagazynieList()
        {
            return new List<LekWMagazynie>
            {
                new LekWMagazynie
                {
                    IdStanLeku = 1,
                    IdLek = 1,
                    Ilosc = 15,
                    DataWaznosci = DateTime.Now.AddYears(1)
                }
            };
        }


        public static List<Choroba> GetChorobaList()
        {
            return new List<Choroba>
            {
                new Choroba
                {
                    IdChoroba = 1,
                    Nazwa = "ból brzucha"
                },
                new Choroba
                {
                    IdChoroba = 2,
                    Nazwa = "ból głowa"
                }
            };
        }


        public static List<ChorobaLek> GetChorobaLekList()
        {
            return new List<ChorobaLek>
            {
                new ChorobaLek
                {
                    IdChoroba = 1,
                    IdLek = 1
                }
            };
        }

        public static List<Usluga> GetUslugaList()
        {
            return new List<Usluga>
            {
                new Usluga
                {
                    IdUsluga = 1,
                    NazwaUslugi = "UslugaName",
                    Narkoza = false,
                    Cena = 120
                },
                new Usluga
                {
                    IdUsluga = 2,
                    NazwaUslugi = "UslugaName",
                    Narkoza = true,
                    Cena = 150
                }
            };
        }

        public static List<Szczepionka> GetSzczepionkaList()
        {
            return new List<Szczepionka>
            {
                new Szczepionka
                {
                    IdLek = 2,
                    CzyObowiazkowa = false,
                    Zastosowanie = "",
                    //OkresWaznosci = new DateTime(10, 10, 10)
                }
            };
        }

        public static List<Szczepienie> GetSzczepienieList()
        {
            return new List<Szczepienie>
            {
                new Szczepienie
                {
                    IdSzczepienie = 1,
                    IdLek = 2,
                    IdPacjent = 1,
                    Dawka = 10,
                    Data = DateTime.Now
                }
            };
        }

        public static List<Znizka> GetZnizkaList()
        {
            return new List<Znizka>
            {
                new Znizka
                {
                    IdZnizka = 1,
                    NazwaZnizki = "Znizka 1",
                    ProcentZnizki = 10.0M
                }
            };
        }

        public static List<KlientZnizka> GetKlientZnizkaList()
        {
            return new List<KlientZnizka>
            {
                new KlientZnizka
                {
                    IdOsoba= 1,
                    IdZnizka = 1,
                    DataPrzyznania = DateTime.Now,
                    CzyWykorzystana = false
                }
            };
        }

        public static List<Harmonogram> GetHarmonogramList()
        {
            return new List<Harmonogram>
            {
                new Harmonogram
                {
                    IdHarmonogram = 1,
                    DataRozpoczecia = new DateTime(2022,10,27,12,0,0),
                    DataZakonczenia = new DateTime(2022,10,27,12,30,0),
                    WeterynarzIdOsoba = 2,
                    IdWizyta = null
                },
                new Harmonogram
                {
                    IdHarmonogram = 2,
                    DataRozpoczecia = new DateTime(2022,10,27,12,30,0),
                    DataZakonczenia = new DateTime(2022,10,27,13,00,0),
                    WeterynarzIdOsoba = 2,
                    IdWizyta = 1
                },
                new Harmonogram
                {
                    IdHarmonogram = 3,
                    DataRozpoczecia = new DateTime(2022,10,27,13,0,0),
                    DataZakonczenia = new DateTime(2022,10,27,13,30,0),
                    WeterynarzIdOsoba = 2,
                    IdWizyta = 1
                },
                new Harmonogram
                {
                    IdHarmonogram = 4,
                    DataRozpoczecia = new DateTime(2022,10,28,15,0,0),
                    DataZakonczenia = new DateTime(2022,10,28,15,30,0),
                    WeterynarzIdOsoba = 2,
                    IdWizyta = 2
                }
            };
        }

        public static List<Wizytum> GetWizytaList()
        {
            return new List<Wizytum>
            {
                new Wizytum
                {
                    IdWizyta = 1,
                    IdOsoba = 1,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString(),
                    Cena = 200,
                    CzyZaakceptowanaCena = false
                },
                new Wizytum
                {
                    IdWizyta = 2,
                    IdOsoba = 1,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zrealizowana.ToString(),
                    Cena = 200,
                    CzyZaakceptowanaCena = true
                },
                new Wizytum
                {
                    IdWizyta = 3,
                    IdOsoba = 4,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum
                {
                    IdWizyta = 4,
                    IdOsoba = 4,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum
                {
                    IdWizyta = 5,
                    IdOsoba = 4,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum
                {
                    IdWizyta = 6,
                    IdOsoba = 4,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum
                {
                    IdWizyta = 7,
                    IdOsoba = 4,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
                new Wizytum
                {
                    IdWizyta = 8,
                    IdOsoba = 4,
                    IdPacjent = 1,
                    Status = WizytaStatus.Zaplanowana.ToString()
                },
            };
        }

        public static List<WizytaChoroba> GetWizytaChorobaList()
        {
            return new List<WizytaChoroba>
            {
                new WizytaChoroba
                {
                    IdWizyta = 1,
                    IdChoroba = 1
                }
            };
        }

        public static List<WizytaLek> GetWizytaLekList()
        {
            return new List<WizytaLek>
            {
                new WizytaLek
                {
                    IdWizyta = 1,
                    IdLek = 1,
                    Ilosc = 1
                }
            };
        }

        public static List<WizytaUsluga> GetWizytaUslugaList()
        {
            return new List<WizytaUsluga>
            {
                new WizytaUsluga
                {
                    IdUsluga = 1,
                    IdWizyta = 1
                }
            };
        }

        public static List<Urlop> GetUrlopList()
        {
            return new List<Urlop>
            {
                new Urlop
                {
                    IdUrlop = 1,
                    IdOsoba = 2,
                    Dzien = new DateTime(2022,11,16)
                }
            };
        }

        public static List<Receptum> GetReceptaList()
        {
            return new List<Receptum>
            {
                new Receptum
                {
                    IdWizyta = 1,
                    Zalecenia = "long text"
                }
            };
        }

        public static List<ReceptaLek> GetReceptaLekList()
        {
            return new List<ReceptaLek>
            {
                new ReceptaLek
                {
                    IdWizyta = 1,
                    IdLek = 1,
                    Ilosc = 5
                }
            };
        }
    }
}