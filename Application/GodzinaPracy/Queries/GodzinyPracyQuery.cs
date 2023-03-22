using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Queries
{
    public class GodzinyPracyQuery : IRequest<List<GodzinyPracyResponse>>
    {
        public string ID_osoba { get; set; }
    }

    public class GodzinyPracyQueryHandle : IRequestHandler<GodzinyPracyQuery, List<GodzinyPracyResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GodzinyPracyQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GodzinyPracyResponse>> Handle(GodzinyPracyQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var results =
                (from x in context.GodzinyPracies
                 where x.IdOsoba == id
                 select new GodzinyPracyResponse()
                 {
                     DzienTygodnia = x.DzienTygodnia,
                     GodzinaRozpoczecia = x.GodzinaRozpoczecia,
                     GodzinaZakonczenia = x.GodzinaZakonczenia
                 }).ToList();

            return results;
        }
    }
}