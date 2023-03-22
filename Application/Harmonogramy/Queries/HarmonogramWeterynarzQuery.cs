using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramWeterynarzQuery : IRequest<object>
    {
        public string ID_osoba { get; set; }
        public DateTime Date { get; set; }
    }

    public class HarmonogramWeterynarzQueryHandle : IRequestHandler<HarmonogramWeterynarzQuery, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public HarmonogramWeterynarzQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<object> Handle(HarmonogramWeterynarzQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);
            var StartDate = req.Date.AddDays(-((int)req.Date.DayOfWeek - 1));
            var EndDate = req.Date.AddDays(7 - (int)req.Date.DayOfWeek);

            var results =
                (from x in context.Harmonograms
                 join z in context.Wizyta on x.IdWizyta equals z.IdWizyta into wizyta from t in wizyta.DefaultIfEmpty()
                 where x.DataRozpoczecia.Date >= StartDate && x.DataZakonczenia.Date <= EndDate && x.WeterynarzIdOsoba == id
                 select new GetHarmonogramAdminResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     IdWizyta = x.IdWizyta != null ? hash.Encode((int)x.IdWizyta) : null,
                     Data = x.DataRozpoczecia,
                     Dzien = (int)x.DataRozpoczecia.DayOfWeek,
                     IdKlient = x.IdWizyta != null ? hash.Encode(t.IdOsoba) : null,
                     Klient = x.IdWizyta != null ? context.Osobas.Where(k => k.IdOsoba == t.IdOsoba).Select(k => k.Imie + " " + k.Nazwisko).First() : null,
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