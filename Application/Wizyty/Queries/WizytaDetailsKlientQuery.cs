using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class WizytaDetailsKlientQuery : IRequest<GetWizytaDetailsKlientResponse>
    {
        public string ID_wizyta { get; set; }
        public string ID_klient { get; set; }
    }

    public class WizytaDetailsKlientQueryHandle : IRequestHandler<WizytaDetailsKlientQuery, GetWizytaDetailsKlientResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IWizyta _wizyta;
        public WizytaDetailsKlientQueryHandle(IKlinikaContext klinikaContext, IHash hash, IWizyta wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
        }

        public async Task<GetWizytaDetailsKlientResponse> Handle(WizytaDetailsKlientQuery req, CancellationToken cancellationToken)
        {
            (int id, int id2) = _hash.Decode(req.ID_wizyta, req.ID_klient);
            var harmonograms = _context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).ToList();

            if (harmonograms.Any())
            {
                (DateTime rozpoczecie, DateTime zakonczenie) = _wizyta.GetWizytaDates(harmonograms);

                return (from x in _context.Wizyta
                        join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                        join d in _context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                        from p in pacjent.DefaultIfEmpty()
                        where x.IdWizyta == id && x.IdOsoba == id2
                        select new GetWizytaDetailsKlientResponse()
                        {
                            Status = x.Status,
                            CzyOplacona = x.CzyOplacona,
                            DataRozpoczecia = rozpoczecie,
                            DataZakonczenia = zakonczenie,
                            Opis = x.Opis,
                            NotatkaKlient = x.NotatkaKlient,
                            Cena = x.CzyZaakceptowanaCena ? (decimal)(x.CenaZnizka != null ? x.CenaZnizka : x.Cena) : null,
                            Weterynarz = _context.Osobas.Where(i => i.IdOsoba.Equals(harmonograms.First().WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First(),
                            IdPacjent = x.IdPacjent != null ? _hash.Encode(p.IdPacjent) : null,
                            Pacjent = x.IdPacjent != null ? p.Nazwa : null
                        }).FirstOrDefault();
            }

            return (from x in _context.Wizyta
                    join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                    join d in _context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                    from p in pacjent.DefaultIfEmpty()
                    where x.IdWizyta == id && x.IdOsoba == id2
                    select new GetWizytaDetailsKlientResponse()
                    {
                        Status = x.Status,
                        CzyOplacona = x.CzyOplacona,
                        DataRozpoczecia = null,
                        DataZakonczenia = null,
                        Opis = x.Opis,
                        NotatkaKlient = x.NotatkaKlient,
                        Cena = x.CzyZaakceptowanaCena ? (decimal)(x.CenaZnizka != null ? x.CenaZnizka : x.Cena) : null,
                        Weterynarz = null,
                        IdPacjent = x.IdPacjent != null ? _hash.Encode((int)x.IdPacjent) : null,
                        Pacjent = x.IdPacjent != null ? p.Nazwa : null
                    }).FirstOrDefault();
        }
    }
}