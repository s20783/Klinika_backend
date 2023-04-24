using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Urlopy.Queries
{
    public class VacationVetQuery : IRequest<List<GetVacationResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class UrlopWeterynarzQueryHandler : IRequestHandler<VacationVetQuery, List<GetVacationResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public UrlopWeterynarzQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetVacationResponse>> Handle(VacationVetQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_weterynarz);

            return _mapper.Map<List<GetVacationResponse>>(await _context.Urlops
                .Include(x => x.IdOsobaNavigation)
                .ThenInclude(x => x.IdOsobaNavigation)
                .Where(x => x.IdOsoba == id)
                .ToListAsync(cancellationToken)
                 );
        }
    }
}