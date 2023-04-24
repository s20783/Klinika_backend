using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class ServiceDetailsQuery : IRequest<GetServiceResponse>
    {
        public string ID_usluga { get; set; }
    }

    public class UslugaDetailsQueryHandler : IRequestHandler<ServiceDetailsQuery, GetServiceResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public UslugaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetServiceResponse> Handle(ServiceDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_usluga);

            return _mapper.Map<GetServiceResponse>(await _context.Uslugas
                .Where(x => x.IdUsluga == id)
                .FirstOrDefaultAsync(cancellationToken)
                );
        }
    }
}