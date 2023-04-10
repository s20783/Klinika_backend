using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class WizytaDetailsAdminQuery : IRequest<GetWizytaDetailsAdminResponse>
    {
        public string ID_wizyta { get; set; }
    }

    public class WizytaDetailsAdminQueryHandle : IRequestHandler<WizytaDetailsAdminQuery, GetWizytaDetailsAdminResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IWizyta _wizyta;
        public WizytaDetailsAdminQueryHandle(IKlinikaContext klinikaContext, IHash hash, IWizyta wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
        }

        public async Task<GetWizytaDetailsAdminResponse> Handle(WizytaDetailsAdminQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);
            var harmonograms = _context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).ToList();

            if (harmonograms.Any())
            {
                (DateTime rozpoczecie, DateTime zakonczenie) = _wizyta.GetWizytaDates(harmonograms);

                return (from x in _context.Wizyta
                        join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                        join d in _context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                        from p in pacjent.DefaultIfEmpty()
                        where x.IdWizyta == id
                        select new GetWizytaDetailsAdminResponse()
                        {
                            Status = x.Status,
                            CzyOplacona = x.CzyOplacona,
                            CzyZaakceptowanaCena = x.CzyZaakceptowanaCena,
                            DataRozpoczecia = rozpoczecie,
                            DataZakonczenia = zakonczenie,
                            Opis = x.Opis,
                            NotatkaKlient = x.NotatkaKlient,
                            Cena = (decimal)(x.CenaZnizka != null ? x.CenaZnizka : x.Cena),
                            Weterynarz = _context.Osobas.Where(i => i.IdOsoba.Equals(harmonograms.First().WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First(),
                            IdWeterynarz = _hash.Encode(harmonograms.First().WeterynarzIdOsoba),
                            IdPacjent = x.IdPacjent != null ? _hash.Encode(p.IdPacjent) : null,
                            Pacjent = x.IdPacjent != null ? p.Nazwa : null,
                            IdKlient = x.IdOsoba != null ? _hash.Encode(k.IdOsoba) : null,
                            Klient = k.Imie + " " + k.Nazwisko
                        }).FirstOrDefault();
            }

            return (from x in _context.Wizyta
                    join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                    join d in _context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                    from p in pacjent.DefaultIfEmpty()
                    where x.IdWizyta == id
                    select new GetWizytaDetailsAdminResponse()
                    {
                        Status = x.Status,
                        CzyOplacona = x.CzyOplacona,
                        CzyZaakceptowanaCena = x.CzyZaakceptowanaCena,
                        DataRozpoczecia = null,
                        DataZakonczenia = null,
                        Opis = x.Opis,
                        NotatkaKlient = x.NotatkaKlient,
                        Cena = (decimal)(x.CenaZnizka != null ? x.CenaZnizka : x.Cena),
                        IdPacjent = x.IdPacjent != null ? _hash.Encode(p.IdPacjent) : null,
                        Pacjent = x.IdPacjent != null ? p.Nazwa : null,
                        IdKlient = x.IdOsoba != null ? _hash.Encode(k.IdOsoba) : null,
                        Klient = k.Imie + " " + k.Nazwisko
                    }).FirstOrDefault();
        }
    }
}