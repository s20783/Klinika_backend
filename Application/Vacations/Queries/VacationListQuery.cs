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
    public class VacationListQuery : IRequest<List<GetVacationResponse>>
    {

    }

    public class UrlopListQueryHandler : IRequestHandler<VacationListQuery, List<GetVacationResponse>>
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

        public async Task<List<GetVacationResponse>> Handle(VacationListQuery req, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<GetVacationResponse>>(await _context.Urlops
                .Include(x => x.IdOsobaNavigation)
                .ThenInclude(x => x.IdOsobaNavigation)
                .OrderByDescending(x => x.Dzien)
                .ToListAsync(cancellationToken)
                );
        }
    }
}