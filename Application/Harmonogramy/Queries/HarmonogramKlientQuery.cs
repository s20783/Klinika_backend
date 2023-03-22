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
    public class HarmonogramKlientQuery : IRequest<List<GetHarmonogramKlientResponse>>
    {
        public DateTime Date { get; set; }
    }

    public class HarmonogramKlientQueryHandle : IRequestHandler<HarmonogramKlientQuery, List<GetHarmonogramKlientResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public HarmonogramKlientQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetHarmonogramKlientResponse>> Handle(HarmonogramKlientQuery req, CancellationToken cancellationToken)
        {
            var results =
                (from x in context.Harmonograms
                 join w in context.Osobas on x.WeterynarzIdOsoba equals w.IdOsoba
                 where x.DataRozpoczecia.Date == req.Date && x.IdWizyta == null
                 orderby x.DataRozpoczecia
                 select new GetHarmonogramKlientResponse()
                 {
                     IdHarmonogram = hash.Encode(x.IdHarmonogram),
                     IdWeterynarz = hash.Encode(x.WeterynarzIdOsoba),
                     Data = x.DataRozpoczecia,
                     Dzien = (int)x.DataRozpoczecia.DayOfWeek,
                     Weterynarz = w.Imie + " " + w.Nazwisko
                 }).ToList();

            return results;
        }
    }
}
