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

namespace Application.Pacjenci.Queries
{
    public class PacjentListQuery : IRequest<PaginatedResponse<GetPacjentListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class GetPacjentListQueryHandle : IRequestHandler<PacjentListQuery, PaginatedResponse<GetPacjentListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetPacjentListResponse> _cache;
        private readonly IMapper _mapper;
        public GetPacjentListQueryHandle(IKlinikaContext klinikaContext, ICache<GetPacjentListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetPacjentListResponse>> Handle(PacjentListQuery req, CancellationToken cancellationToken)
        {
            List<GetPacjentListResponse> data;
            data = _cache.GetFromCache();

            if(data is null)
            {
                data = _mapper.Map<List<GetPacjentListResponse>>(await _context.Pacjents
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