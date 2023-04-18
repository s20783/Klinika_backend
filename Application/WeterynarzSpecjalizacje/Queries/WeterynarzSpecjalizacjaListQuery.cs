﻿using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WeterynarzSpecjalizacje.Queries
{
    public class WeterynarzSpecjalizacjaListQuery : IRequest<List<GetSpecjalizacjaResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandler : IRequestHandler<WeterynarzSpecjalizacjaListQuery, List<GetSpecjalizacjaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public SpecjalizacjaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetSpecjalizacjaResponse>> Handle(WeterynarzSpecjalizacjaListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_weterynarz);

            return _mapper.Map<List<GetSpecjalizacjaResponse>>(await _context.WeterynarzSpecjalizacjas
                .Include(x => x.IdSpecjalizacjaNavigation)
                .Where(x => x.IdOsoba == id)
                .OrderBy(x => x.IdSpecjalizacjaNavigation.Nazwa)
                .ToListAsync(cancellationToken)
                );
        }
    }
}