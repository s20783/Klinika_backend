using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Queries
{
    public class KlientListAllQuery : IRequest<List<GetKlientListResponse>>
    {
        
    }

    public class KlientListAllQueryHandler : IRequestHandler<KlientListAllQuery, List<GetKlientListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetKlientListResponse> _cache;
        private readonly IMapper _mapper;
        public KlientListAllQueryHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetKlientListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetKlientListResponse>> Handle(KlientListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetKlientListResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetKlientListResponse>>(await _context.Klients
                    .Include(x => x.IdOsobaNavigation)
                    .Where(x => x.IdOsobaNavigation.IdRola == ((int)RolaEnum.Klient))
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            return data
                .OrderBy(x => x.Nazwisko)
                .ToList();
        }
    }
}