using Application.DTO.Responses;
using Application.Interfaces;
using Application.ReceptaLeki.Queries;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class MapperProfile : Profile
    {
        private readonly IHash _hash;
        public MapperProfile(IHash hash)
        {
            _hash = hash;

            CreateMap<ChorobaLek, GetDiseaseResponse>()
                .ForMember(x => x.ID_Choroba, y => y.MapFrom(s => _hash.Encode(s.IdChoroba)))
                .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdChorobaNavigation.Nazwa))
                .ForMember(x => x.Opis, y => y.MapFrom(s => s.IdChorobaNavigation.Opis))
                .ForMember(x => x.NazwaLacinska, y => y.MapFrom(s => s.IdChorobaNavigation.NazwaLacinska));

            CreateMap<Choroba, GetDiseaseResponse>()
                .ForMember(x => x.ID_Choroba, y => y.MapFrom(s => _hash.Encode(s.IdChoroba)));

            CreateMap<GodzinyPracy, GetWorkingHoursResponse>();

            CreateMap<Klient, GetClientListResponse>()
                .ForMember(x => x.IdOsoba, y => y.MapFrom(s => _hash.Encode(s.IdOsoba)))
                .ForMember(x => x.Imie, y => y.MapFrom(s => s.IdOsobaNavigation.Imie))
                .ForMember(x => x.NumerTelefonu, y => y.MapFrom(s => s.IdOsobaNavigation.NumerTelefonu))
                .ForMember(x => x.Nazwisko, y => y.MapFrom(s => s.IdOsobaNavigation.Nazwisko))
                .ForMember(x => x.Email, y => y.MapFrom(s => s.IdOsobaNavigation.Email));

            CreateMap<Osoba, GetClientResponse>()
                .ForMember(x => x.DataZalozeniaKonta, y => y.MapFrom(s => s.Klient.DataZalozeniaKonta));

            CreateMap<Osoba, GetAccountResponse>();

            CreateMap<Lek, GetMedicamentResponse>()
                .ForMember(x => x.IdLek, y => y.MapFrom(s => _hash.Encode(s.IdLek)));

            CreateMap<LekWMagazynie, GetMedicamentWarehouseResponse>();

            CreateMap<Pacjent, GetPatientListResponse>()
                .ForMember(x => x.IdPacjent, y => y.MapFrom(s => _hash.Encode(s.IdPacjent)))
                .ForMember(x => x.IdOsoba, y => y.MapFrom(s => _hash.Encode(s.IdOsoba)))
                .ForMember(x => x.Wlasciciel, y => y.MapFrom(s => s.IdOsobaNavigation.IdOsobaNavigation.Imie + " " + s.IdOsobaNavigation.IdOsobaNavigation.Nazwisko));

            CreateMap<Pacjent, GetPatientDetailsResponse>()
                .ForMember(x => x.IdOsoba, y => y.MapFrom(s => _hash.Encode(s.IdOsoba)))
                .ForMember(x => x.Wlasciciel, y => y.MapFrom(s => s.IdOsobaNavigation.IdOsobaNavigation.Imie + " " + s.IdOsobaNavigation.IdOsobaNavigation.Nazwisko));

            CreateMap<Pacjent, GetPatientKlientListResponse>()
                .ForMember(x => x.IdPacjent, y => y.MapFrom(s => _hash.Encode(s.IdPacjent)));

            CreateMap<ReceptaLek, GetPrescriptionMedicamentResponse>()
                .ForMember(x => x.ID_Lek, y => y.MapFrom(s => _hash.Encode(s.IdLek)))
                .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdLekNavigation.Nazwa))
                .ForMember(x => x.JednostkaMiary, y => y.MapFrom(s => s.IdLekNavigation.JednostkaMiary))
                .ForMember(x => x.Producent, y => y.MapFrom(s => s.IdLekNavigation.Producent));

            CreateMap<Specjalizacja, GetSpecializationResponse>()
                .ForMember(x => x.IdSpecjalizacja, y => y.MapFrom(s => _hash.Encode(s.IdSpecjalizacja)));

            CreateMap<Szczepienie, GetVaccinationResponse>()
                .ForMember(x => x.IdSzczepienie, y => y.MapFrom(s => _hash.Encode(s.IdSzczepienie)))
                .ForMember(x => x.IdPacjent, y => y.MapFrom(s => _hash.Encode(s.IdPacjent)))
                .ForMember(x => x.IdLek, y => y.MapFrom(s => _hash.Encode(s.IdLek)))
                .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdLekNavigation.IdLekNavigation.Nazwa))
                .ForMember(x => x.DataWaznosci, y => y.MapFrom(s => getDate(s.Data, (long)s.IdLekNavigation.OkresWaznosci)))
                .ForMember(x => x.Dawka, y => y.MapFrom(s => s.Dawka))
                .ForMember(x => x.Data, y => y.MapFrom(s => s.Data));

            CreateMap<Szczepionka, GetVaccineResponse>()
                .ForMember(x => x.ID_lek, y => y.MapFrom(s => _hash.Encode(s.IdLek)))
                .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdLekNavigation.Nazwa))
                .ForMember(x => x.Producent, y => y.MapFrom(s => s.IdLekNavigation.Producent))
                .ForMember(x => x.OkresWaznosci, y => y.MapFrom(s => getDate(s.OkresWaznosci)));

            CreateMap<Urlop, GetVacationResponse>()
               .ForMember(x => x.IdUrlop, y => y.MapFrom(s => _hash.Encode(s.IdUrlop)))
               .ForMember(x => x.Weterynarz, y => y.MapFrom(s => s.IdOsobaNavigation.IdOsobaNavigation.Imie + " " + s.IdOsobaNavigation.IdOsobaNavigation.Nazwisko));

            CreateMap<Usluga, GetServiceResponse>()
               .ForMember(x => x.ID_Usluga, y => y.MapFrom(s => _hash.Encode(s.IdUsluga)));

            CreateMap<WizytaUsluga, GetServiceResponse>()
               .ForMember(x => x.ID_Usluga, y => y.MapFrom(s => _hash.Encode(s.IdUsluga)))
               .ForMember(x => x.Dolegliwosc, y => y.MapFrom(s => s.IdUslugaNavigation.Dolegliwosc))
               .ForMember(x => x.NazwaUslugi, y => y.MapFrom(s => s.IdUslugaNavigation.NazwaUslugi))
               .ForMember(x => x.Opis, y => y.MapFrom(s => s.IdUslugaNavigation.Opis))
               .ForMember(x => x.Cena, y => y.MapFrom(s => s.IdUslugaNavigation.Cena))
               .ForMember(x => x.Narkoza, y => y.MapFrom(s => s.IdUslugaNavigation.Narkoza));

            CreateMap<WizytaUsluga, GetServicePatientResponse>()
               .ForMember(x => x.ID_Usluga, y => y.MapFrom(s => _hash.Encode(s.IdUsluga)))
               .ForMember(x => x.ID_wizyta, y => y.MapFrom(s => _hash.Encode(s.IdWizyta)))
               .ForMember(x => x.NazwaUslugi, y => y.MapFrom(s => s.IdUslugaNavigation.NazwaUslugi))
               .ForMember(x => x.Opis, y => y.MapFrom(s => s.IdUslugaNavigation.Opis))
               .ForMember(x => x.Data, y => y.MapFrom(s => s.IdWizytaNavigation.Harmonograms.Min(x => x.DataRozpoczecia)));

            CreateMap<Osoba, GetVetListResponse>()
               .ForMember(x => x.IdOsoba, y => y.MapFrom(s => _hash.Encode(s.IdOsoba)));

            CreateMap<Osoba, GetVetResponse>()
               .ForMember(x => x.DataZatrudnienia, y => y.MapFrom(s => s.Weterynarz.DataZatrudnienia))
               .ForMember(x => x.DataUrodzenia, y => y.MapFrom(s => s.Weterynarz.DataUrodzenia))
               .ForMember(x => x.Pensja, y => y.MapFrom(s => s.Weterynarz.Pensja));

            CreateMap<WeterynarzSpecjalizacja, GetSpecializationResponse>()
               .ForMember(x => x.IdSpecjalizacja, y => y.MapFrom(s => _hash.Encode(s.IdSpecjalizacja)))
               .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdSpecjalizacjaNavigation.Nazwa))
               .ForMember(x => x.Opis, y => y.MapFrom(s => s.IdSpecjalizacjaNavigation.Opis));

            CreateMap<WizytaChoroba, GetDiseaseResponse>()
               .ForMember(x => x.ID_Choroba, y => y.MapFrom(s => _hash.Encode(s.IdChoroba)))
               .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdChorobaNavigation.Nazwa))
               .ForMember(x => x.Opis, y => y.MapFrom(s => s.IdChorobaNavigation.Opis))
               .ForMember(x => x.NazwaLacinska, y => y.MapFrom(s => s.IdChorobaNavigation.NazwaLacinska));

            CreateMap<WizytaLek, GetMedicamentListResponse>()
               .ForMember(x => x.IdLek, y => y.MapFrom(s => _hash.Encode(s.IdLek)))
               .ForMember(x => x.Nazwa, y => y.MapFrom(s => s.IdLekNavigation.Nazwa))
               .ForMember(x => x.JednostkaMiary, y => y.MapFrom(s => s.IdLekNavigation.JednostkaMiary))
               .ForMember(x => x.Producent, y => y.MapFrom(s => s.IdLekNavigation.Producent));
        }

        public DateTime? getDate(DateTime? a, long b)
        {
            if(a == null)
            {
                return null;
            }
            return ((DateTime)a).AddTicks(b);
        }

        public int? getDate(long? a)
        {
            if (a == null)
            {
                return null;
            }
            return TimeSpan.FromTicks((long)a).Days;
        }
    }
}
