using Application.DTO.Responses;
using Application.Interfaces;
using Domain;
using MediatR;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class UslugaListQuery : IRequest<PaginatedResponse<GetUslugaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class UslugaListQueryHandler : IRequestHandler<UslugaListQuery, PaginatedResponse<GetUslugaResponse>>
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

        public async Task<PaginatedResponse<GetUslugaResponse>> Handle(UslugaListQuery req, CancellationToken cancellationToken)
        {
            List<GetUslugaResponse> data = cache.GetFromCache();

            if (data is null)
            {
                data = (from x in context.Uslugas
                        select new GetUslugaResponse()
                        {
                            ID_Usluga = hash.Encode(x.IdUsluga),
                            NazwaUslugi = x.NazwaUslugi,
                            Opis = x.Opis,
                            Cena = x.Cena,
                            Narkoza = x.Narkoza,
                            Dolegliwosc = x.Dolegliwosc
                        }).ToList();

                cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.NazwaUslugi.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Opis.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Dolegliwosc.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.NazwaUslugi);

            return new PaginatedResponse<GetUslugaResponse>
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