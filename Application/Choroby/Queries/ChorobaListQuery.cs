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

namespace Application.Choroby.Queries
{
    public class ChorobaListQuery : IRequest<PaginatedResponse<GetChorobaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class ChorobaListQueryHandler : IRequestHandler<ChorobaListQuery, PaginatedResponse<GetChorobaResponse>>
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

        public async Task<PaginatedResponse<GetChorobaResponse>> Handle(ChorobaListQuery req, CancellationToken cancellationToken)
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

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Opis.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwa);


            return new PaginatedResponse<GetChorobaResponse>
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