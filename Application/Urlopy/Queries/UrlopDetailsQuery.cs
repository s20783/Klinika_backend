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
    public class UrlopDetailsQuery : IRequest<GetUrlopResponse>
    {
        public string ID_urlop { get; set; }
    }

    public class UrlopDetailsQueryHandler : IRequestHandler<UrlopDetailsQuery, GetUrlopResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UrlopDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetUrlopResponse> Handle(UrlopDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_urlop);

            return (from x in context.Urlops
                 join w in context.Osobas on x.IdOsoba equals w.IdOsoba
                 join y in context.Osobas on w.IdOsoba equals y.IdOsoba
                 where x.IdUrlop == id
                 select new GetUrlopResponse()
                 {
                     IdUrlop = req.ID_urlop,
                     ID_Weterynarz = hash.Encode(x.IdOsoba),
                     Weterynarz = y.Imie + " " + y.Nazwisko,
                     Dzien = x.Dzien
                 }).First();
        }
    }
}