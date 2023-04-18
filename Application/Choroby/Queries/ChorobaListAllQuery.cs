using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Queries
{
    public class ChorobaListAllQuery : IRequest<List<GetChorobaResponse>>
    {
        
    }

    public class ChorobaListAllQueryHandler : IRequestHandler<ChorobaListAllQuery, List<GetChorobaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetChorobaResponse> _cache;
        private readonly IMapper _mapper;
        public ChorobaListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetChorobaResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetChorobaResponse>> Handle(ChorobaListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetChorobaResponse> data;
            data = _cache.GetFromCache();

            if(data is null)
            {
                data = _mapper.Map<List<GetChorobaResponse>>(await _context.Chorobas
                    .OrderBy(x => x.Nazwa)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data.OrderBy(x => x.Nazwa).ToList();
        }
    }
}