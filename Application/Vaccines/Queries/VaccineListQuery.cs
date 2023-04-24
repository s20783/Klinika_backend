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

namespace Application.Szczepionki.Queries
{
    public class VaccineListQuery : IRequest<PaginatedResponse<GetVaccineResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class SzczepionkaListQueryHandler : IRequestHandler<VaccineListQuery, PaginatedResponse<GetVaccineResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IMapper _mapper;
        public SzczepionkaListQueryHandler(IKlinikaContext klinikaContext, IMapper mapper)
        {
            _context = klinikaContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetVaccineResponse>> Handle(VaccineListQuery req, CancellationToken cancellationToken)
        {     
            var results = _mapper.Map<List<GetVaccineResponse>>(await _context.Szczepionkas
                .Include(x => x.IdLekNavigation)
                .Where(
                x => req.SearchWord == null ||
                x.IdLekNavigation.Nazwa.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Zastosowanie.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.IdLekNavigation.Producent.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.IdLekNavigation.Nazwa)
                .ToListAsync(cancellationToken)
                );

            return new PaginatedResponse<GetVaccineResponse>
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