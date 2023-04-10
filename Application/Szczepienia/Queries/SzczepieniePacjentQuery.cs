﻿using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Queries
{
    public class SzczepieniePacjentQuery : IRequest<List<GetSzczepienieResponse>>
    {
        public string ID_pacjent { get; set; }
    }

    public class SzczepieniePacjentQueryHandler : IRequestHandler<SzczepieniePacjentQuery, List<GetSzczepienieResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public SzczepieniePacjentQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetSzczepienieResponse>> Handle(SzczepieniePacjentQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_pacjent);

            return _mapper.Map<List<GetSzczepienieResponse>>(await _context.Szczepienies
                .Include(x => x.IdLekNavigation)
                .ThenInclude(x => x.IdLekNavigation)
                .Where(x => x.IdPacjent == id)
                .OrderBy(x => x.IdLekNavigation.IdLekNavigation.Nazwa)
                .ToListAsync(cancellationToken)
                );
        }
    }
}