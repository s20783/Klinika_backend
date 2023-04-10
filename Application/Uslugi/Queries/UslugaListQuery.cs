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
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class UslugaListQuery : IRequest<PaginatedResponse<GetUslugaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class UslugaListQueryHandler : IRequestHandler<UslugaListQuery, PaginatedResponse<GetUslugaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetUslugaResponse> _cache;
        private readonly IMapper _mapper;
        public UslugaListQueryHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetUslugaResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetUslugaResponse>> Handle(UslugaListQuery req, CancellationToken cancellationToken)
        {
            List<GetUslugaResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetUslugaResponse>>(_context.Uslugas
                    .OrderBy(x => x.NazwaUslugi)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.NazwaUslugi.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Opis.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Dolegliwosc.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.NazwaUslugi);

            return new PaginatedResponse<GetUslugaResponse>
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