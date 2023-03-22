using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Urlopy.Queries
{
    public class UrlopWeterynarzQuery : IRequest<List<GetUrlopResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class UrlopWeterynarzQueryHandler : IRequestHandler<UrlopWeterynarzQuery, List<GetUrlopResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UrlopWeterynarzQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetUrlopResponse>> Handle(UrlopWeterynarzQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_weterynarz);

            var results =
                (from x in context.Urlops
                 join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                 where x.IdOsoba == id
                 select new GetUrlopResponse()
                 {
                     IdUrlop = hash.Encode(x.IdUrlop),
                     ID_Weterynarz = req.ID_weterynarz,
                     Weterynarz = y.Imie + " " + y.Nazwisko,
                     Dzien = x.Dzien
                 }).ToList();

            return results;
        }
    }
}