﻿using Application.DTO.Responses;
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
    public class SzczepionkaListQuery : IRequest<PaginatedResponse<GetSzczepionkaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class SzczepionkaListQueryHandler : IRequestHandler<SzczepionkaListQuery, PaginatedResponse<GetSzczepionkaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public SzczepionkaListQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetSzczepionkaResponse>> Handle(SzczepionkaListQuery req, CancellationToken cancellationToken)
        {     
            var results = _mapper.Map<List<GetSzczepionkaResponse>>(await _context.Szczepionkas
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