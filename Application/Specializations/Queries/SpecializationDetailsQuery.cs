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

namespace Application.Specjalizacje.Queries
{
    public class SpecializationDetailsQuery : IRequest<GetSpecializationResponse>
    {
        public string ID_specjalizacja { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandler : IRequestHandler<SpecializationDetailsQuery, GetSpecializationResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public SpecjalizacjaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetSpecializationResponse> Handle(SpecializationDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_specjalizacja);

            return _mapper.Map<GetSpecializationResponse>(await _context.Specjalizacjas
                    .Where(x => x.IdSpecjalizacja == id)
                    .FirstOrDefaultAsync(cancellationToken)
                    );
        }
    }
}