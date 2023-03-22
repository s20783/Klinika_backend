using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class LekOnlyListQuery : IRequest<List<GetLekResponse>>
    {

    }

    public class LekOnlyListQueryHandler : IRequestHandler<LekOnlyListQuery, List<GetLekResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public LekOnlyListQueryHandler(IKlinikaContext _context, IHash _hash)
        {
            context = _context;
            hash = _hash;
        }

        public async Task<List<GetLekResponse>> Handle(LekOnlyListQuery req, CancellationToken cancellationToken)
        {
            return context.Leks
                .Where(x => x.Szczepionka == null)
                .Select(x => new GetLekResponse {
                    IdLek = hash.Encode(x.IdLek),
                    Nazwa = x.Nazwa,
                    Producent = x.Producent,
                    JednostkaMiary = x.JednostkaMiary,
                })
                .OrderBy(x => x.Nazwa)
                .ToList();
        }
    }
}
