﻿using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Queries
{
    public class GodzinyPracyQuery : IRequest<List<GetGodzinyPracyResponse>>
    {
        public string ID_osoba { get; set; }
    }

    public class GodzinyPracyQueryHandle : IRequestHandler<GodzinyPracyQuery, List<GetGodzinyPracyResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public GodzinyPracyQueryHandle(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetGodzinyPracyResponse>> Handle(GodzinyPracyQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            return _mapper.Map<List<GetGodzinyPracyResponse>>(await _context.GodzinyPracies
                .Where(x => x.IdOsoba == id)
                .ToListAsync(cancellationToken)
                );
        }
    }
}