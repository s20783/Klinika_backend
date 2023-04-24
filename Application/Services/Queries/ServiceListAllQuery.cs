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
    public class ServiceListAllQuery : IRequest<List<GetServiceResponse>>
    {

    }

    public class UslugaListAllQueryHandler : IRequestHandler<ServiceListAllQuery, List<GetServiceResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetServiceResponse> _cache;
        private readonly IMapper _mapper;
        public UslugaListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetServiceResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetServiceResponse>> Handle(ServiceListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetServiceResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetServiceResponse>>(await _context.Uslugas
                    .OrderBy(x => x.NazwaUslugi)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data.OrderBy(x => x.NazwaUslugi).ToList();
        }
    }
}