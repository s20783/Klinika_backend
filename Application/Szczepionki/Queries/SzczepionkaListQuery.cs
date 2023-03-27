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

namespace Application.Szczepionki.Queries
{
    public class SzczepionkaListQuery : IRequest<PaginatedResponse<GetSzczepionkaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class SzczepionkaListQueryHandler : IRequestHandler<SzczepionkaListQuery, PaginatedResponse<GetSzczepionkaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SzczepionkaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<PaginatedResponse<GetSzczepionkaResponse>> Handle(SzczepionkaListQuery req, CancellationToken cancellationToken)
        {
            var data = (from x in context.Szczepionkas
                    join y in context.Leks on x.IdLek equals y.IdLek
                    orderby y.Nazwa
                    select new GetSzczepionkaResponse()
                    {
                        ID_lek = hash.Encode(y.IdLek),
                        Nazwa = y.Nazwa,
                        Producent = y.Producent,
                        CzyObowiazkowa = x.CzyObowiazkowa,
                        OkresWaznosci = x.OkresWaznosci != null ? TimeSpan.FromTicks((long)x.OkresWaznosci).Days : null,
                        Zastosowanie = x.Zastosowanie
                    }).ToList();

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Zastosowanie.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Producent.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwa);

            return new PaginatedResponse<GetSzczepionkaResponse>
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