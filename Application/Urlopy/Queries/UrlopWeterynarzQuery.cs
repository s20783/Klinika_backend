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
    public class UrlopWeterynarzQuery : IRequest<List<GetUrlopResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class UrlopWeterynarzQueryHandler : IRequestHandler<UrlopWeterynarzQuery, List<GetUrlopResponse>>
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

        public async Task<List<GetUrlopResponse>> Handle(UrlopWeterynarzQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_weterynarz);

            return _mapper.Map<List<GetUrlopResponse>>(await _context.Urlops
                .Include(x => x.IdOsobaNavigation)
                .ThenInclude(x => x.IdOsobaNavigation)
                .Where(x => x.IdOsoba == id)
                .FirstOrDefaultAsync(cancellationToken)
                 );
        }
    }
}