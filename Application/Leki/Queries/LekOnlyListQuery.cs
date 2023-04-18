﻿using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class LekOnlyListQuery : IRequest<List<GetLekResponse>>
    {

    }

    public class LekOnlyListQueryHandler : IRequestHandler<LekOnlyListQuery, List<GetLekResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IMapper _mapper;
        public LekOnlyListQueryHandler(IKlinikaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetLekResponse>> Handle(LekOnlyListQuery req, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<GetLekResponse>>(await _context.Leks
                .Where(x => x.Szczepionka == null)
                .OrderBy(x => x.Nazwa)
                .ToListAsync(cancellationToken)
                );
        }
    }
}
