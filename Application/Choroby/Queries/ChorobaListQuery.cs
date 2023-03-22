using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Queries
{
    public class ChorobaListQuery : IRequest<List<GetChorobaResponse>>
    {

    }

    public class ChorobaListQueryHandler : IRequestHandler<ChorobaListQuery, List<GetChorobaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetChorobaResponse> cache;
        public ChorobaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash, ICache<GetChorobaResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<List<GetChorobaResponse>> Handle(ChorobaListQuery req, CancellationToken cancellationToken)
        {
            List<GetChorobaResponse> data;
            data = cache.GetFromCache();

            if(data is null)
            {
                data = (from x in context.Chorobas
                        orderby x.Nazwa
                        select new GetChorobaResponse()
                        {
                            ID_Choroba = hash.Encode(x.IdChoroba),
                            Nazwa = x.Nazwa,
                            NazwaLacinska = x.NazwaLacinska,
                            Opis = x.Opis
                        }).AsParallel().WithCancellation(cancellationToken).ToList();

                cache.AddToCache(data);
            }

            return data;
        }
    }
}