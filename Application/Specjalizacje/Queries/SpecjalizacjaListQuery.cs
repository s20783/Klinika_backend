using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Queries
{
    public class SpecjalizacjaListQuery : IRequest<List<GetSpecjalizacjaResponse>>
    {

    }

    public class SpecjalizacjaListQueryHandle : IRequestHandler<SpecjalizacjaListQuery, List<GetSpecjalizacjaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetSpecjalizacjaResponse> cache;
        public SpecjalizacjaListQueryHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetSpecjalizacjaResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<List<GetSpecjalizacjaResponse>> Handle(SpecjalizacjaListQuery req, CancellationToken cancellationToken)
        {
            List<GetSpecjalizacjaResponse> data = cache.GetFromCache();

            if (data is null)
            {
                data = (from x in context.Specjalizacjas
                        orderby x.Nazwa
                        select new GetSpecjalizacjaResponse()
                        {
                            IdSpecjalizacja = hash.Encode(x.IdSpecjalizacja),
                            Nazwa = x.Nazwa,
                            Opis = x.Opis
                        }).AsParallel().WithCancellation(cancellationToken).ToList();
            }

            return data;
        }
    }
}