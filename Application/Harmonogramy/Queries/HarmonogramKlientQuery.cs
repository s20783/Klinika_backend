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
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public HarmonogramKlientQueryHandle(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<List<GetHarmonogramKlientResponse>> Handle(HarmonogramKlientQuery req, CancellationToken cancellationToken)
        {
            var results =
                (from x in _context.Harmonograms
                 join w in _context.Osobas on x.WeterynarzIdOsoba equals w.IdOsoba
                 where x.DataRozpoczecia.Date == req.Date && x.IdWizyta == null
                 orderby x.DataRozpoczecia
                 select new GetHarmonogramKlientResponse()
                 {
                     IdHarmonogram = _hash.Encode(x.IdHarmonogram),
                     IdWeterynarz = _hash.Encode(x.WeterynarzIdOsoba),
                     Data = x.DataRozpoczecia,
                     Dzien = (int)x.DataRozpoczecia.DayOfWeek,
                     Weterynarz = w.Imie + " " + w.Nazwisko
                 }).ToList();

            return results;
        }
    }
}
