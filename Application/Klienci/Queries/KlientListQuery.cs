using Application.DTO.Responses;
using Application.Interfaces;
using Domain;
using Domain.Enums;
using MediatR;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Queries
{
    public class KlientListQuery : IRequest<PaginatedResponse<GetKlientListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class KlientListQueryHandle : IRequestHandler<KlientListQuery, PaginatedResponse<GetKlientListResponse>>
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

        public async Task<PaginatedResponse<GetKlientListResponse>> Handle(KlientListQuery req, CancellationToken cancellationToken)
        {
            List<GetKlientListResponse> data = cache.GetFromCache();

            if (data is null)
            {
                data = (from x in context.Klients
                        join y in context.Osobas on x.IdOsoba equals y.IdOsoba into ps
                        from p in ps
                        where p.IdRola == ((int)RolaEnum.Klient)
                        select new GetKlientListResponse()
                        {
                            IdOsoba = hash.Encode(x.IdOsoba),
                            Imie = p.Imie,
                            Nazwisko = p.Nazwisko,
                            NumerTelefonu = p.NumerTelefonu,
                            Email = p.Email
                        }).AsParallel().WithCancellation(cancellationToken).ToList();

                cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwisko.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Imie.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.NumerTelefonu.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Email.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwisko);

            return new PaginatedResponse<GetKlientListResponse>
            {
                Items = results
                    .Skip((req.Page - 1) * GlobalValues.PAGE_SIZE)
                    .Take(GlobalValues.PAGE_SIZE)
                    .ToList(),
                PageCount = (int)Math.Ceiling(results.Count() / (double)GlobalValues.PAGE_SIZE),
                PageIndex = req.Page
            };
        }
    }
}