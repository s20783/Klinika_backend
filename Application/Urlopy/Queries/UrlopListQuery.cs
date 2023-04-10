using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Urlopy.Queries
{
    public class UrlopListQuery : IRequest<List<GetUrlopResponse>>
    {

    }

    public class UrlopListQueryHandler : IRequestHandler<UrlopListQuery, List<GetUrlopResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public UrlopListQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetUrlopResponse>> Handle(UrlopListQuery req, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<GetUrlopResponse>>(await _context.Urlops
                .Include(x => x.IdOsobaNavigation)
                .ThenInclude(x => x.IdOsobaNavigation)
                .OrderByDescending(x => x.Dzien)
                .ToListAsync(cancellationToken)
                );
        }
    }
}