using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Queries
{
    public class SzczepienieDetailsQuery : IRequest<GetSzczepienieResponse>
    {
        public string ID_szczepienie { get; set; }
    }

    public class SzczepienieDetailsQueryHandler : IRequestHandler<SzczepienieDetailsQuery, GetSzczepienieResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public SzczepienieDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetSzczepienieResponse> Handle(SzczepienieDetailsQuery req, CancellationToken cancellationToken)
        { 
            int id = _hash.Decode(req.ID_szczepienie);

            return _mapper.Map<GetSzczepienieResponse>(await _context.Szczepienies
                .Include(x => x.IdLekNavigation)
                .ThenInclude(x => x.IdLekNavigation)
                .Where(x => x.IdSzczepienie == id)
                .FirstOrDefaultAsync(cancellationToken)
                );     
        }
    }
}