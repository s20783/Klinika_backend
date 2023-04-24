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
    public class SpecializationListAllQuery : IRequest<List<GetSpecializationResponse>>
    {

    }

    public class SpecjalizacjaListAllQueryHandler : IRequestHandler<SpecializationListAllQuery, List<GetSpecializationResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetSpecializationResponse> _cache;
        private readonly IMapper _mapper;
        public SpecjalizacjaListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetSpecializationResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetSpecializationResponse>> Handle(SpecializationListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetSpecializationResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetSpecializationResponse>>(await _context.Specjalizacjas
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data.OrderBy(x => x.Nazwa).ToList();
        }
    }
}