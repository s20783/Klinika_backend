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

namespace Application.Szczepionki.Queries
{
    public class VaccineDetailsQuery : IRequest<GetVaccineResponse>
    {
        public string ID_szczepionka { get; set; }
    }

    public class SzczepionkaDetailsQueryHandler : IRequestHandler<VaccineDetailsQuery, GetVaccineResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public SzczepionkaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetVaccineResponse> Handle(VaccineDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_szczepionka);

            return _mapper.Map<GetVaccineResponse>(await _context.Szczepionkas
                .Include(x => x.IdLekNavigation)
                .Where(x => x.IdLek == id)
                .FirstOrDefaultAsync(cancellationToken)
                );
        }
    }
}