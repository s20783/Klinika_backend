using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Queries
{
    public class KlientListQuery : IRequest<List<GetKlientListResponse>>
    {

    }

    public class KlientListQueryHandle : IRequestHandler<KlientListQuery, List<GetKlientListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetKlientListResponse> cache;
        public KlientListQueryHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetKlientListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<List<GetKlientListResponse>> Handle(KlientListQuery req, CancellationToken cancellationToken)
        {
            List<GetKlientListResponse> data = cache.GetFromCache();

            if (data is null)
            {
                data = (from x in context.Klients
                        join y in context.Osobas on x.IdOsoba equals y.IdOsoba into ps
                        from p in ps
                        where p.Rola == null
                        orderby p.Nazwisko, p.Imie
                        select new GetKlientListResponse()
                        {
                            IdOsoba = hash.Encode(x.IdOsoba),
                            Imie = p.Imie,
                            Nazwisko = p.Nazwisko,
                            NumerTelefonu = p.NumerTelefonu,
                            Email = p.Email
                        }).AsParallel().WithCancellation(cancellationToken).ToList();
            }

            return data;
        }
    }
}