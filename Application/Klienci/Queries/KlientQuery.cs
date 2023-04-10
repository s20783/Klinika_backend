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

namespace Application.Klienci.Queries
{
    public class KlientQuery : IRequest<GetKlientResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetKlientQueryHandle : IRequestHandler<KlientQuery, GetKlientResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public GetKlientQueryHandle(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetKlientResponse> Handle(KlientQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            return _mapper.Map<GetKlientResponse>(await _context.Osobas
                .Include(x => x.Klient)
                .Where(x => x.IdOsoba == id)
                .FirstOrDefaultAsync(cancellationToken)
                );
        }
    }
}