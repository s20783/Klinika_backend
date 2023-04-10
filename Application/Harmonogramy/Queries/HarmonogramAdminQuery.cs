using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramAdminQuery : IRequest<object>
    {
        public DateTime Date { get; set; }
    }

    public class HarmonogramAdminQueryHandle : IRequestHandler<HarmonogramAdminQuery, object>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public HarmonogramAdminQueryHandle(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<object> Handle(HarmonogramAdminQuery req, CancellationToken cancellationToken)
        {
            var StartDate = req.Date.AddDays(-((int)req.Date.DayOfWeek - 1));
            var EndDate = req.Date.AddDays(7 - (int)req.Date.DayOfWeek);

            var results =
                (from x in _context.Harmonograms
                 join z in _context.Wizyta on x.IdWizyta equals z.IdWizyta into wizyta
                 from t in wizyta.DefaultIfEmpty()
                 join w in _context.Osobas on x.WeterynarzIdOsoba equals w.IdOsoba
                 where x.DataRozpoczecia.Date >= StartDate && x.DataZakonczenia.Date <= EndDate
                 select new GetHarmonogramAdminResponse()
                 {
                     IdHarmonogram = _hash.Encode(x.IdHarmonogram),
                     IdWizyta = x.IdWizyta != null ? _hash.Encode((int)x.IdWizyta) : null,
                     IdWeterynarz = _hash.Encode(x.WeterynarzIdOsoba),
                     Weterynarz = w.Imie + " " + w.Nazwisko,
                     Dzien = (int)x.DataRozpoczecia.DayOfWeek,
                     Data = x.DataRozpoczecia,
                     IdKlient = x.IdWizyta != null ? _hash.Encode(t.IdOsoba) : null,
                     Klient = x.IdWizyta != null ? _context.Osobas.Where(k => k.IdOsoba == t.IdOsoba).Select(k => k.Imie + " " + k.Nazwisko).First() : null,
                     //IdPacjent = x.IdWizyta != null ? hash.Encode((int)t.IdPacjent) : null,
                     //Pacjent = x.IdWizyta != null ? context.Pacjents.Where(p => p.IdPacjent == t.IdPacjent).Select(p => p.Nazwa).FirstOrDefault() : null,
                     CzyZajete = x.IdWizyta != null
                 }).AsParallel().WithCancellation(cancellationToken).OrderBy(x => x.Data).ToList();

            return new 
            {
                Start = StartDate,
                End = EndDate,
                harmonogramy = results
            };
        }
    }
}
