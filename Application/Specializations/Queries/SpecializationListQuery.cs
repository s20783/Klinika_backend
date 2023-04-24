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

namespace Application.Specjalizacje.Queries
{
    public class SpecializationListQuery : IRequest<PaginatedResponse<GetSpecializationResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class SpecjalizacjaListQueryHandler : IRequestHandler<SpecializationListQuery, PaginatedResponse<GetSpecializationResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetSpecializationResponse> _cache;
        private readonly IMapper _mapper;
        public SpecjalizacjaListQueryHandler(IKlinikaContext klinikaContext, ICache<GetSpecializationResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetSpecializationResponse>> Handle(SpecializationListQuery req, CancellationToken cancellationToken)
        {
            List<GetSpecializationResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetSpecializationResponse>>(await _context.Specjalizacjas
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Opis.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwa)
                .ThenBy(x => x.Opis);

            return new PaginatedResponse<GetSpecializationResponse>
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