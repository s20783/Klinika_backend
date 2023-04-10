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
    public class UrlopDetailsQuery : IRequest<GetUrlopResponse>
    {
        public string ID_urlop { get; set; }
    }

    public class UrlopDetailsQueryHandler : IRequestHandler<UrlopDetailsQuery, GetUrlopResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public UrlopDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetUrlopResponse> Handle(UrlopDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_urlop);

            return _mapper.Map<GetUrlopResponse>(await _context.Urlops
                .Include(x => x.IdOsobaNavigation)
                .ThenInclude(x => x.IdOsobaNavigation)
                .Where(x => x.IdUrlop == id)
                .FirstOrDefaultAsync(cancellationToken)
                 );
        }
    }
}