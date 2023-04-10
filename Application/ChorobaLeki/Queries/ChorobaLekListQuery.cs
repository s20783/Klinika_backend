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

namespace Application.ChorobaLeki.Queries
{
    public class ChorobaLekListQuery : IRequest<List<GetChorobaResponse>>
    {
        public string ID_lek { get; set; }
    }

    public class ChorobaLekListQueryHandle : IRequestHandler<ChorobaLekListQuery, List<GetChorobaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public ChorobaLekListQueryHandle(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<List<GetChorobaResponse>> Handle(ChorobaLekListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_lek);

            var data = await _context.ChorobaLeks
                .Include(x => x.IdChorobaNavigation)
                .Where(x => x.IdLek == id)
                .OrderBy(x => x.IdChorobaNavigation.Nazwa)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GetChorobaResponse>>(data);
        }
    }
}