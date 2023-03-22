using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class UslugaListQuery : IRequest<List<GetUslugaResponse>>
    {

    }

    public class UslugaListQueryHandler : IRequestHandler<UslugaListQuery, List<GetUslugaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetUslugaResponse> cache;
        public UslugaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash, ICache<GetUslugaResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<List<GetUslugaResponse>> Handle(UslugaListQuery req, CancellationToken cancellationToken)
        {
            List<GetUslugaResponse> data = cache.GetFromCache();

            if (data is null)
            {
                data = (from x in context.Uslugas
                        orderby x.NazwaUslugi
                        select new GetUslugaResponse()
                        {
                            ID_Usluga = hash.Encode(x.IdUsluga),
                            NazwaUslugi = x.NazwaUslugi,
                            Opis = x.Opis,
                            Cena = x.Cena,
                            Narkoza = x.Narkoza,
                            Dolegliwosc = x.Dolegliwosc
                        }).ToList();
            }

            return data;
        }
    }
}