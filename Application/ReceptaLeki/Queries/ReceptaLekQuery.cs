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

namespace Application.ReceptaLeki.Queries
{
    public class ReceptaLekQuery : IRequest<List<GetReceptaLekResponse>>
    {
        public string ID_Recepta { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandler : IRequestHandler<ReceptaLekQuery, List<GetReceptaLekResponse>>
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

        public async Task<List<GetReceptaLekResponse>> Handle(ReceptaLekQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Recepta);

            return _mapper.Map<List<GetReceptaLekResponse>>(await _context.ReceptaLeks
                .Include(x => x.IdLekNavigation)
                .Where(x => x.IdWizyta == id)
                .OrderBy(x => x.IdLekNavigation.Nazwa)
                .ToListAsync(cancellationToken)
                );
        }
    }
}