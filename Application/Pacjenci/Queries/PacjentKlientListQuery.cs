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

namespace Application.Pacjenci.Queries
{
    public class PacjentKlientListQuery : IRequest<List<GetPacjentKlientListResponse>>
    {
        public string ID_osoba { get; set; }
    }

    public class PacjentKlientListQueryHandler : IRequestHandler<PacjentKlientListQuery, List<GetPacjentKlientListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public PacjentKlientListQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }


        public async Task<List<GetPacjentKlientListResponse>> Handle(PacjentKlientListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            return _mapper.Map<List<GetPacjentKlientListResponse>>(await _context.Pacjents
                    .Where(x => x.IdOsoba == id)
                    .OrderBy(x => x.Nazwa)
                    .ToListAsync(cancellationToken)
                    );
        }
    }
}