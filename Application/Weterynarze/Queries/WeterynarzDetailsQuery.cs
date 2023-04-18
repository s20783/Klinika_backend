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
    public class WeterynarzDetailsQuery : IRequest<GetWeterynarzResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetWeterynarzQueryHandler : IRequestHandler<WeterynarzDetailsQuery, GetWeterynarzResponse>
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

        public async Task<GetWeterynarzResponse> Handle(WeterynarzDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            return _mapper.Map<GetWeterynarzResponse>(await _context.Osobas
                .Include(x => x.Weterynarz)
                .Where(x => x.IdOsoba == id)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}