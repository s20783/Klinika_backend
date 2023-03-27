using Application.DTO.Responses;
using Application.Interfaces;
using Domain;
using MediatR;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Queries
{
    public class SpecjalizacjaListQuery : IRequest<PaginatedResponse<GetSpecjalizacjaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class SpecjalizacjaListQueryHandle : IRequestHandler<SpecjalizacjaListQuery, PaginatedResponse<GetSpecjalizacjaResponse>>
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

        public async Task<PaginatedResponse<GetSpecjalizacjaResponse>> Handle(SpecjalizacjaListQuery req, CancellationToken cancellationToken)
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

                cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Opis.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwa);

            return new PaginatedResponse<GetSpecjalizacjaResponse>
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