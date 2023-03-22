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
    public class SzczepionkaListQuery : IRequest<List<GetSzczepionkaResponse>>
    {

    }

    public class SzczepionkaListQueryHandler : IRequestHandler<SzczepionkaListQuery, List<GetSzczepionkaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SzczepionkaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetSzczepionkaResponse>> Handle(SzczepionkaListQuery req, CancellationToken cancellationToken)
        {
            return (from x in context.Szczepionkas
                    join y in context.Leks on x.IdLek equals y.IdLek
                    orderby y.Nazwa
                    select new GetSzczepionkaResponse()
                    {
                        ID_lek = hash.Encode(y.IdLek),
                        Nazwa = y.Nazwa,
                        Producent = y.Producent,
                        CzyObowiazkowa = x.CzyObowiazkowa,
                        OkresWaznosci = x.OkresWaznosci != null ? TimeSpan.FromTicks((long)x.OkresWaznosci).Days : null,
                        Zastosowanie = x.Zastosowanie
                    }).ToList();
        }
    }
}