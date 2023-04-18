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

namespace Application.Uslugi.Queries
{
    public class UslugaListAllQuery : IRequest<List<GetUslugaResponse>>
    {

    }

    public class UslugaListAllQueryHandler : IRequestHandler<UslugaListAllQuery, List<GetUslugaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetUslugaResponse> _cache;
        private readonly IMapper _mapper;
        public UslugaListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetUslugaResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetUslugaResponse>> Handle(UslugaListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetUslugaResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetUslugaResponse>>(await _context.Uslugas
                    .OrderBy(x => x.NazwaUslugi)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data.OrderBy(x => x.NazwaUslugi).ToList();
        }
    }
}