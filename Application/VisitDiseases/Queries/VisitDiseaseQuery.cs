﻿using Application.DTO.Responses;
using Application.Interfaces;
using Application.ReceptaLeki.Queries;
using Application.Recepty.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Queries
{
    public class VisitDiseaseQuery : IRequest<List<GetDiseaseResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class WizytaChorobaQueryHandler : IRequestHandler<VisitDiseaseQuery, List<GetDiseaseResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public WizytaChorobaQueryHandler(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetDiseaseResponse>> Handle(VisitDiseaseQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            return _mapper.Map<List<GetDiseaseResponse>>(await _context.WizytaChorobas
                .Include(x => x.IdChorobaNavigation)
                .Where(x => x.IdWizyta == id)
                .ToListAsync(cancellationToken)
                );
        }
    }
}