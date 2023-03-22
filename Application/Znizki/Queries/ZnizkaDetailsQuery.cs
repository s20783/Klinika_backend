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
    public class ZnizkaDetailsQuery : IRequest<GetZnizkaResponse>
    {
        public string ID_znizka { get; set; }
    }

    public class ZnizkaDetailsQueryHandler : IRequestHandler<ZnizkaDetailsQuery, GetZnizkaResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public ZnizkaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetZnizkaResponse> Handle(ZnizkaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_znizka);

            return (from x in context.Znizkas
                    where x.IdZnizka == id
                    select new GetZnizkaResponse()
                    {
                        ID_Znizka = hash.Encode(x.IdZnizka),
                        NazwaZnizki = x.NazwaZnizki,
                        ProcentZnizki = x.ProcentZnizki
                    }).First();
        }
    }
}