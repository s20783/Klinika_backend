using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class MedicamentDetailsQuery : IRequest<object>
    {
        public string ID_lek { get; set; }
    }

    public class LekQueryHandler : IRequestHandler<MedicamentDetailsQuery, object>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public LekQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<object> Handle(MedicamentDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_lek);

            var t = _context.Leks.Where(x => x.IdLek.Equals(id)).First();

            if (!_context.LekWMagazynies.Where(x => x.IdLek.Equals(id)).Any())
            {
                return new {
                    Nazwa = t.Nazwa,
                    JednostkaMiary = t.JednostkaMiary,
                    Producent = t.Producent,
                    Choroby = Enumerable.Empty<object>().ToList(),
                    LekList = Enumerable.Empty<GetMedicamentWarehouseResponse>().ToList()
                };
            }

            return new
            {
                Nazwa = t.Nazwa,
                JednostkaMiary = t.JednostkaMiary,
                Producent = t.Producent,
                Choroby = (from i in _context.ChorobaLeks
                           join j in _context.Chorobas on i.IdChoroba equals j.IdChoroba into qs
                           from j in qs.DefaultIfEmpty()
                           where i.IdLek == id
                           select new
                           {
                               IdChoroba = _hash.Encode(j.IdChoroba),
                               Nazwa = j.Nazwa,
                               NazwaLacinska = j.NazwaLacinska
                           }).ToList(),
                LekList = (from y in _context.LekWMagazynies
                           where y.IdLek == id
                           select new GetMedicamentWarehouseResponse()
                           {
                               IdStanLeku = _hash.Encode(y.IdStanLeku),
                               Ilosc = y.Ilosc,
                               DataWaznosci = y.DataWaznosci,
                           }).ToList()
            };
        }
    }
}