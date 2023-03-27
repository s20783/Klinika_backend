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

namespace Application.Weterynarze.Queries
{
    public class WeterynarzListQuery : IRequest<PaginatedResponse<GetWeterynarzListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class WeterynarzListQueryHandler : IRequestHandler<WeterynarzListQuery, PaginatedResponse<GetWeterynarzListResponse>>
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

        public async Task<PaginatedResponse<GetWeterynarzListResponse>> Handle(WeterynarzListQuery req, CancellationToken cancellationToken)
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

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Imie.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Nazwisko.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.NumerTelefonu.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Email.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwisko);

            return new PaginatedResponse<GetWeterynarzListResponse>
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