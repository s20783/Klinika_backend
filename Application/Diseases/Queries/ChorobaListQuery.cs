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
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Queries
{
    public class ChorobaListQuery : IRequest<PaginatedResponse<GetDiseaseResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class ChorobaListQueryHandler : IRequestHandler<ChorobaListQuery, PaginatedResponse<GetDiseaseResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetDiseaseResponse> _cache;
        private readonly IMapper _mapper;
        public ChorobaListQueryHandler(IKlinikaContext klinikaContext, ICache<GetDiseaseResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetDiseaseResponse>> Handle(ChorobaListQuery req, CancellationToken cancellationToken)
        {
            List<GetDiseaseResponse> data;
            data = _cache.GetFromCache();

            if(data is null)
            {
                data = _mapper.Map<List<GetDiseaseResponse>>(await _context.Chorobas
                    .OrderBy(x => x.Nazwa)
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
                .OrderBy(x => x.Nazwa);


            return new PaginatedResponse<GetDiseaseResponse>
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