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
    public class ClientListAllQuery : IRequest<List<GetClientListResponse>>
    {
        
    }

    public class KlientListAllQueryHandler : IRequestHandler<ClientListAllQuery, List<GetClientListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetClientListResponse> _cache;
        private readonly IMapper _mapper;
        public KlientListAllQueryHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetClientListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<GetClientListResponse>> Handle(ClientListAllQuery req, CancellationToken cancellationToken)
        {
            List<GetClientListResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetClientListResponse>>(await _context.Klients
                    .Include(x => x.IdOsobaNavigation)
                    .Where(x => x.IdOsobaNavigation.IdRola == ((int)RoleEnum.Klient))
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