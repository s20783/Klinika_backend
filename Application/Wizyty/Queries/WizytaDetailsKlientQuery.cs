using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public WizytaDetailsKlientQueryHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository wizyta)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = wizyta;
        }

        public async Task<GetWizytaDetailsKlientResponse> Handle(WizytaDetailsKlientQuery req, CancellationToken cancellationToken)
        {
            (int id, int id2) = hash.Decode(req.ID_wizyta, req.ID_klient);
            var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).ToList();

            if (harmonograms.Any())
            {
                (DateTime rozpoczecie, DateTime zakonczenie) = wizytaRepository.GetWizytaDates(harmonograms);

                return (from x in context.Wizyta
                        join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                        join d in context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
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
                            Weterynarz = context.Osobas.Where(i => i.IdOsoba.Equals(harmonograms.First().WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First(),
                            IdPacjent = x.IdPacjent != null ? hash.Encode(p.IdPacjent) : null,
                            Pacjent = x.IdPacjent != null ? p.Nazwa : null
                        }).FirstOrDefault();
            }

            return (from x in context.Wizyta
                    join k in context.Osobas on x.IdOsoba equals k.IdOsoba
                    join d in context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
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
                        IdPacjent = x.IdPacjent != null ? hash.Encode((int)x.IdPacjent) : null,
                        Pacjent = x.IdPacjent != null ? p.Nazwa : null
                    }).FirstOrDefault();
        }
    }
}