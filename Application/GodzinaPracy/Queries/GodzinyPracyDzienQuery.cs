using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Queries
{
    public class GodzinyPracyDzienQuery : IRequest<GodzinyPracyResponse>
    {
        public string ID_osoba { get; set; }
        public int Dzien { get; set; }
    }

    public class GodzinyPracyDzienQueryHandler : IRequestHandler<GodzinyPracyDzienQuery, GodzinyPracyResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GodzinyPracyDzienQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GodzinyPracyResponse> Handle(GodzinyPracyDzienQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            return (from x in context.GodzinyPracies
                    where x.IdOsoba == id && x.DzienTygodnia == req.Dzien
                    select new GodzinyPracyResponse()
                    {
                        DzienTygodnia = x.DzienTygodnia,
                        GodzinaRozpoczecia = x.GodzinaRozpoczecia,
                        GodzinaZakonczenia = x.GodzinaZakonczenia
                    }).FirstOrDefault();
        }
    }
}