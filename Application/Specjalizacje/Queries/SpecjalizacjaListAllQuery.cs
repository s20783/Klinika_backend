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
    public class SpecjalizacjaListAllQuery : IRequest<List<GetSpecjalizacjaResponse>>
    {

    }

    public class SpecjalizacjaListAllQueryHandler : IRequestHandler<SpecjalizacjaListAllQuery, List<GetSpecjalizacjaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetSpecjalizacjaResponse> _cache;
        private readonly IMapper _mapper;
        public SpecjalizacjaListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetSpecjalizacjaResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetSpecjalizacjaResponse>> Handle(SpecjalizacjaListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetSpecjalizacjaResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetSpecjalizacjaResponse>>(await _context.Specjalizacjas
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data.OrderBy(x => x.Nazwa).ToList();
        }
    }
}