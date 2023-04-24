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

namespace Application.Pacjenci.Queries
{
    public class PatientListQuery : IRequest<PaginatedResponse<GetPatientListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class GetPacjentListQueryHandle : IRequestHandler<PatientListQuery, PaginatedResponse<GetPatientListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetPatientListResponse> _cache;
        private readonly IMapper _mapper;
        public GetPacjentListQueryHandle(IKlinikaContext klinikaContext, ICache<GetPatientListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetPatientListResponse>> Handle(PatientListQuery req, CancellationToken cancellationToken)
        {
            List<GetPatientListResponse> data;
            data = _cache.GetFromCache();

            if(data is null)
            {
                data = _mapper.Map<List<GetPatientListResponse>>(await _context.Pacjents
                    .Include(x => x.IdOsobaNavigation)
                    .ThenInclude(x => x.IdOsobaNavigation)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
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

            return new PaginatedResponse<GetPatientListResponse>
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