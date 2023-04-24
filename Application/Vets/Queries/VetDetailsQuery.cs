using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Queries
{
    public class VetDetailsQuery : IRequest<GetVetResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetWeterynarzQueryHandler : IRequestHandler<VetDetailsQuery, GetVetResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public GetWeterynarzQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetVetResponse> Handle(VetDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            return _mapper.Map<GetVetResponse>(await _context.Osobas
                .Include(x => x.Weterynarz)
                .Where(x => x.IdOsoba == id)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}