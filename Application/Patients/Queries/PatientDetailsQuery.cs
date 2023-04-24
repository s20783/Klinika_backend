using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PatientDetailsQuery : IRequest<GetPatientDetailsResponse>
    {
        public string ID_pacjent { get; set; }
    }

    public class PacjentDetailsQueryHandle : IRequestHandler<PatientDetailsQuery, GetPatientDetailsResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public PacjentDetailsQueryHandle(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetPatientDetailsResponse> Handle(PatientDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_pacjent);

            return _mapper.Map<GetPatientDetailsResponse>(await _context.Pacjents
                    .Include(x => x.IdOsobaNavigation)
                    .ThenInclude(x => x.IdOsobaNavigation)
                    .Where(x => x.IdPacjent == id)
                    .FirstAsync(cancellationToken)
                    );
        }
    }
}