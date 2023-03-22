using System;

namespace Domain
{
    public static class GlobalValues
    {
        public readonly static int DNI_PRACY = 5;
        public readonly static TimeSpan GODZINA_ROZPOCZECIA_PRACY = new TimeSpan(9, 0, 0);
        public readonly static TimeSpan GODZINA_ZAKONCZENIA_PRACY = new TimeSpan(17, 0, 0);
        public readonly static int MAX_UMOWIONYCH_WIZYT = 5;
        public readonly static int GODZINY_DO_ANULOWANIA_WIZYTY_BEZ_KONSEKWENCJI = 4;
        public readonly static int LICZBA_PROB = 12;
        public readonly static int GODZINY_BLOKADY = 1;
        public readonly static int DLUGOSC_WIZYTY = 30;
        public readonly static string SZCZEPIONKA_JEDNOSTKA = "ml";
    }
}