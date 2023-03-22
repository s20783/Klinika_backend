using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepionki.Queries
{
    public class SzczepionkaDetailsQuery : IRequest<GetSzczepionkaResponse>
    {
        public string ID_szczepionka { get; set; }
    }

    public class SzczepionkaDetailsQueryHandler : IRequestHandler<SzczepionkaDetailsQuery, GetSzczepionkaResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SzczepionkaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetSzczepionkaResponse> Handle(SzczepionkaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_szczepionka);

            return (from x in context.Szczepionkas
                    join y in context.Leks on x.IdLek equals y.IdLek
                    where x.IdLek == id
                    select new GetSzczepionkaResponse()
                    {
                        ID_lek = hash.Encode(y.IdLek),
                        Nazwa = y.Nazwa,
                        Producent = y.Producent,
                        CzyObowiazkowa = x.CzyObowiazkowa,
                        OkresWaznosci = x.OkresWaznosci != null ? TimeSpan.FromTicks((long)x.OkresWaznosci).Days : null,
                        Zastosowanie = x.Zastosowanie
                    }).First();
        }
    }
}