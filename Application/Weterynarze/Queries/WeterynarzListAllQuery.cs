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
    public class WeterynarzListAllQuery : IRequest<List<GetWeterynarzListResponse>>
    {
        
    }

    public class WeterynarzListAllQueryHandler : IRequestHandler<WeterynarzListAllQuery, List<GetWeterynarzListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetWeterynarzListResponse> _cache;
        private readonly IMapper _mapper;
        public WeterynarzListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetWeterynarzListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetWeterynarzListResponse>> Handle(WeterynarzListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetWeterynarzListResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetWeterynarzListResponse>>(await _context.Osobas
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