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
    public class ChorobaListAllQuery : IRequest<List<GetDiseaseResponse>>
    {
        
    }

    public class ChorobaListAllQueryHandler : IRequestHandler<ChorobaListAllQuery, List<GetDiseaseResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetDiseaseResponse> _cache;
        private readonly IMapper _mapper;
        public ChorobaListAllQueryHandler(IKlinikaContext klinikaContext, ICache<GetDiseaseResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetDiseaseResponse>> Handle(ChorobaListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetDiseaseResponse> data;
            data = _cache.GetFromCache();

            if(data is null)
            {
                data = _mapper.Map<List<GetDiseaseResponse>>(await _context.Chorobas
                    .OrderBy(x => x.Nazwa)
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data.OrderBy(x => x.Nazwa).ToList();
        }
    }
}