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

namespace Application.WizytaLeki.Queries
{
    public class WizytaLekListQuery : IRequest<List<GetLekListResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class WizytaLekListQueryHandler : IRequestHandler<WizytaLekListQuery, List<GetLekListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public WizytaLekListQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetLekListResponse>> Handle(WizytaLekListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            return _mapper.Map<List<GetLekListResponse>>(await _context.WizytaLeks
                .Include(x => x.IdLekNavigation)
                .Where(x => x.IdWizyta == id)
                .OrderBy(x => x.IdLekNavigation.Nazwa)
                .ToListAsync(cancellationToken));
        }
    }
}