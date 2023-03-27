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

namespace Application.Pacjenci.Queries
{
    public class PacjentListQuery : IRequest<PaginatedResponse<GetPacjentListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class GetPacjentListQueryHandle : IRequestHandler<PacjentListQuery, PaginatedResponse<GetPacjentListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetPacjentListResponse> cache;
        public GetPacjentListQueryHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetPacjentListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<PaginatedResponse<GetPacjentListResponse>> Handle(PacjentListQuery req, CancellationToken cancellationToken)
        {
            List<GetPacjentListResponse> data;
            data = cache.GetFromCache();

            if(data is null)
            {
                data = (from x in context.Pacjents
                        join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                        orderby x.Nazwa
                        select new GetPacjentListResponse()
                        {
                            IdOsoba = hash.Encode(x.IdOsoba),
                            IdPacjent = hash.Encode(x.IdPacjent),
                            Nazwa = x.Nazwa,
                            Gatunek = x.Gatunek,
                            Rasa = x.Rasa,
                            Plec = x.Plec,
                            Wlasciciel = y.Imie + ' ' + y.Nazwisko
                        }).ToList();

                cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Gatunek.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Rasa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Wlasciciel.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwa);

            return new PaginatedResponse<GetPacjentListResponse>
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