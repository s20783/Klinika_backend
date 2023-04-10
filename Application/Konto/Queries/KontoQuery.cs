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

namespace Application.Konto.Queries
{
    public class KontoQuery : IRequest<GetKontoResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetKontoQueryHandle : IRequestHandler<KontoQuery, GetKontoResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public GetKontoQueryHandle(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetKontoResponse> Handle(KontoQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            return _mapper.Map<GetKontoResponse>(await _context.Osobas
                .Where(x => x.IdOsoba == id)
                .FirstOrDefaultAsync(cancellationToken)
                );
        }
    }
}