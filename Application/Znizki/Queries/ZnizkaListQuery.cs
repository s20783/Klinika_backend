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

namespace Application.Znizki.Queries
{
    public class ZnizkaListQuery : IRequest<List<GetZnizkaResponse>>
    {

    }

    public class SzczepionkaListQueryHandler : IRequestHandler<ZnizkaListQuery, List<GetZnizkaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public SzczepionkaListQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<List<GetZnizkaResponse>> Handle(ZnizkaListQuery req, CancellationToken cancellationToken)
        {
            return await (from x in _context.Znizkas
                    orderby x.NazwaZnizki
                    select new GetZnizkaResponse()
                    {
                        ID_Znizka = _hash.Encode(x.IdZnizka),
                        NazwaZnizki = x.NazwaZnizki,
                        ProcentZnizki = x.ProcentZnizki
                    }).ToListAsync(cancellationToken);
        }
    }
}