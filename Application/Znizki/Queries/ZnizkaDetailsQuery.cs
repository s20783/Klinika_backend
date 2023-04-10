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
    public class ZnizkaDetailsQuery : IRequest<GetZnizkaResponse>
    {
        public string ID_znizka { get; set; }
    }

    public class ZnizkaDetailsQueryHandler : IRequestHandler<ZnizkaDetailsQuery, GetZnizkaResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public ZnizkaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<GetZnizkaResponse> Handle(ZnizkaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_znizka);

            return await (from x in _context.Znizkas
                    where x.IdZnizka == id
                    select new GetZnizkaResponse()
                    {
                        ID_Znizka = _hash.Encode(x.IdZnizka),
                        NazwaZnizki = x.NazwaZnizki,
                        ProcentZnizki = x.ProcentZnizki
                    }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}