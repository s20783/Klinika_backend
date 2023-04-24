using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Queries
{
    public class MedicamentWarehouseQuery : IRequest<GetMedicamentWarehouseResponse>
    {
        public string ID_stan_leku { get; set; }
    }

    public class GetStanLekuQueryHandle : IRequestHandler<MedicamentWarehouseQuery, GetMedicamentWarehouseResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IMapper _mapper;
        public GetStanLekuQueryHandle(IKlinikaContext klinikaContext, IHash hash, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<GetMedicamentWarehouseResponse> Handle(MedicamentWarehouseQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_stan_leku);

            return _mapper.Map<GetMedicamentWarehouseResponse>(await _context.LekWMagazynies
                .Where(x => x.IdStanLeku == id)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}