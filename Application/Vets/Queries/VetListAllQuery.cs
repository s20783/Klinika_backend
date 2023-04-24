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

namespace Application.Weterynarze.Queries
{
    public class VetListAllQuery : IRequest<List<GetVetListResponse>>
    {
        
    }

    public class WeterynarzListAllQueryHandler : IRequestHandler<VetListAllQuery, List<GetVetListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetVetListResponse> _cache;
        private readonly IMapper _mapper;
        public WeterynarzListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetVetListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetVetListResponse>> Handle(VetListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetVetListResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetVetListResponse>>(await _context.Osobas
                    .Include(x => x.Weterynarz)
                    .Where(x => x.Weterynarz != null)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data
                .OrderBy(x => x.Nazwisko)
                .ThenBy(x => x.Imie)
                .ToList();
        }
    }
}