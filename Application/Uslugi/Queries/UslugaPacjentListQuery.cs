﻿using Application.DTO.Responses;
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
    public class UslugaPacjentListQuery : IRequest<List<GetUslugaPacjentResponse>>
    {
        public string ID_pacjent { get; set; }
    }

    public class UslugaPacjentListQueryHandler : IRequestHandler<UslugaPacjentListQuery, List<GetUslugaPacjentResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public UslugaPacjentListQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetUslugaPacjentResponse>> Handle(UslugaPacjentListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_pacjent);

            return _mapper.Map<List<GetUslugaPacjentResponse>>(await _context.WizytaUslugas
                .Include(x => x.IdUslugaNavigation)
                .Include(x => x.IdWizytaNavigation)
                .ThenInclude(x => x.Harmonograms)
                .Where(x => x.IdWizytaNavigation.IdPacjent == id)
                .ToListAsync(cancellationToken)
                );
        }
    }
}