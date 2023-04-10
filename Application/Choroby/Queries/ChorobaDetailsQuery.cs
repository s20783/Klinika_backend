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

namespace Application.Choroby.Queries
{
    public class ChorobaDetailsQuery : IRequest<GetChorobaResponse>
    {
        public string ID_Choroba { get; set; }
    }

    public class ChorobaDetailsQueryHandler : IRequestHandler<ChorobaDetailsQuery, GetChorobaResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public ChorobaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetChorobaResponse> Handle(ChorobaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Choroba);

            return _mapper.Map<GetChorobaResponse>(await _context.Chorobas
                    .OrderBy(x => x.Nazwa)
                    .Where(x => x.IdChoroba == id)
                    .FirstAsync(cancellationToken)
                    );
        }
    }
}