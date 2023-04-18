using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepionki.Queries
{
    public class SzczepionkaListAllQuery : IRequest<List<GetSzczepionkaResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class SzczepionkaListAllQueryHandler : IRequestHandler<SzczepionkaListAllQuery, List<GetSzczepionkaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IMapper _mapper;
        public SzczepionkaListAllQueryHandler(IKlinikaContext klinikaContext, IMapper mapper)
        {
            _context = klinikaContext;
            _mapper = mapper;
        }

        public async Task<List<GetSzczepionkaResponse>> Handle(SzczepionkaListAllQuery req, CancellationToken cancellationToken)
        {     
            return _mapper.Map<List<GetSzczepionkaResponse>>(await _context.Szczepionkas
                .Include(x => x.IdLekNavigation)
                .OrderBy(x => x.IdLekNavigation.Nazwa)
                .ToListAsync(cancellationToken)
                );
        }
    }
}