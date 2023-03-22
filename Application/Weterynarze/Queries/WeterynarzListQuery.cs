using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Queries
{
    public class WeterynarzListQuery : IRequest<List<GetWeterynarzListResponse>>
    {

    }

    public class WeterynarzListQueryHandler : IRequestHandler<WeterynarzListQuery, List<GetWeterynarzListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetWeterynarzListResponse> cache;
        public WeterynarzListQueryHandler(IKlinikaContext klinikaContext, IHash _hash, ICache<GetWeterynarzListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<List<GetWeterynarzListResponse>> Handle(WeterynarzListQuery req, CancellationToken cancellationToken)
        {
            List<GetWeterynarzListResponse> data = cache.GetFromCache();

            if (data is null)
            {
                data =
                (from x in context.Osobas
                 join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                 from p in ps
                 select new GetWeterynarzListResponse()
                 {
                     IdOsoba = hash.Encode(x.IdOsoba),
                     Imie = x.Imie,
                     Nazwisko = x.Nazwisko,
                     NumerTelefonu = x.NumerTelefonu,
                     Email = x.Email,
                     DataZatrudnienia = p.DataZatrudnienia
                 }).AsParallel().WithCancellation(cancellationToken).ToList();
            }

            return data;
        }
    }
}