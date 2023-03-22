using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Znizki.Queries
{
    public class ZnizkaListQuery : IRequest<List<GetZnizkaResponse>>
    {

    }

    public class SzczepionkaListQueryHandler : IRequestHandler<ZnizkaListQuery, List<GetZnizkaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SzczepionkaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetZnizkaResponse>> Handle(ZnizkaListQuery req, CancellationToken cancellationToken)
        {
            return (from x in context.Znizkas
                    orderby x.NazwaZnizki
                    select new GetZnizkaResponse()
                    {
                        ID_Znizka = hash.Encode(x.IdZnizka),
                        NazwaZnizki = x.NazwaZnizki,
                        ProcentZnizki = x.ProcentZnizki
                    }).ToList();
        }
    }
}