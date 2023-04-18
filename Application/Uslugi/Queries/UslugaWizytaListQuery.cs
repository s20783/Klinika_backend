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

namespace Application.Uslugi.Queries
{
    public class UslugaWizytaListQuery : IRequest<List<GetUslugaResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class UslugaWizytaListQueryHandler : IRequestHandler<UslugaWizytaListQuery, List<GetUslugaResponse>>
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

        public async Task<List<GetUslugaResponse>> Handle(UslugaWizytaListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            return _mapper.Map<List<GetUslugaResponse>>(await _context.WizytaUslugas
                .Include(x => x.IdUslugaNavigation)
                .Where(x => x.IdWizyta == id)
                .OrderBy(x => x.IdUslugaNavigation.NazwaUslugi)
                .ToListAsync(cancellationToken)
                );
        }
    }
}