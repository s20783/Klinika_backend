using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Queries
{
    public class VetListQuery : IRequest<PaginatedResponse<GetVetListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class WeterynarzListQueryHandler : IRequestHandler<VetListQuery, PaginatedResponse<GetVetListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetVetListResponse> _cache;
        private readonly IMapper _mapper;
        public WeterynarzListQueryHandler(IKlinikaContext klinikaContext, ICache<GetVetListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetVetListResponse>> Handle(VetListQuery req, CancellationToken cancellationToken)
        {
            List<GetVetListResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetVetListResponse>>(await _context.Osobas
                    .Include(x => x.Weterynarz)
                    .Where(x => x.Weterynarz != null)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
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

            return new PaginatedResponse<GetVetListResponse>
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