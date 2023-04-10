using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramAdminByIDQuery : IRequest<object>
    {
        public string ID_osoba { get; set; }
        public DateTime Date { get; set; }
    }

    public class HarmonogramAdminByIDQueryHandler : IRequestHandler<HarmonogramAdminByIDQuery, object>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public HarmonogramAdminByIDQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<object> Handle(HarmonogramAdminByIDQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);
            var StartDate = req.Date.AddDays(-((int)req.Date.DayOfWeek - 1));
            var EndDate = req.Date.AddDays(7 - (int)req.Date.DayOfWeek);

            var results = await
                (from x in _context.Harmonograms
                 join z in _context.Wizyta on x.IdWizyta equals z.IdWizyta into wizyta
                 from t in wizyta.DefaultIfEmpty()
                 where x.DataRozpoczecia.Date >= StartDate && x.DataZakonczenia.Date <= EndDate && x.WeterynarzIdOsoba == id                
                 select new GetHarmonogramAdminResponse()
                 {
                     IdHarmonogram = _hash.Encode(x.IdHarmonogram),
                     IdWizyta = x.IdWizyta != null ? _hash.Encode((int)x.IdWizyta) : null,
                     //IdWeterynarz = req.ID_osoba,
                     //Weterynarz = w.Imie + " " + w.Nazwisko,
                     Data = x.DataRozpoczecia,
                     Dzien = ((int)x.DataRozpoczecia.DayOfWeek),
                     IdKlient = x.IdWizyta != null ? _hash.Encode(t.IdOsoba) : null,
                     Klient = x.IdWizyta != null ? _context.Osobas.Where(k => k.IdOsoba == t.IdOsoba).Select(k => k.Imie + " " + k.Nazwisko).First() : null,
                     IdPacjent = t.IdPacjent != null ? _hash.Encode((int)t.IdPacjent) : null,
                     Pacjent = t.IdPacjent != null ? _context.Pacjents.Where(p => p.IdPacjent == t.IdPacjent).Select(p => p.Nazwa).First() : null,
                     CzyZajete = x.IdWizyta != null
                 }).OrderBy(x => x.Data).ToListAsync(cancellationToken);

            return new
            {
                Start = StartDate,
                End = EndDate,
                harmonogramy = results
            };
        }
    }
}

