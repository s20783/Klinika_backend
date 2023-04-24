using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class ServiceVisitListQuery : IRequest<List<GetServiceResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class UslugaWizytaListQueryHandler : IRequestHandler<ServiceVisitListQuery, List<GetServiceResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public UslugaWizytaListQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetServiceResponse>> Handle(ServiceVisitListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            return _mapper.Map<List<GetServiceResponse>>(await _context.WizytaUslugas
                .Include(x => x.IdUslugaNavigation)
                .Where(x => x.IdWizyta == id)
                .OrderBy(x => x.IdUslugaNavigation.NazwaUslugi)
                .ToListAsync(cancellationToken)
                );
        }
    }
}