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
    public class UrlopListQuery : IRequest<List<GetUrlopResponse>>
    {

    }

    public class UrlopListQueryHandler : IRequestHandler<UrlopListQuery, List<GetUrlopResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UrlopListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetUrlopResponse>> Handle(UrlopListQuery req, CancellationToken cancellationToken)
        {
            var results =
                (from x in context.Urlops
                 join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                 select new GetUrlopResponse()
                 {
                     IdUrlop = hash.Encode(x.IdUrlop),
                     ID_Weterynarz = hash.Encode(x.IdOsoba),
                     Weterynarz = y.Imie + " " + y.Nazwisko,
                     Dzien = x.Dzien
                 }).ToList();

            return results;
        }
    }
}