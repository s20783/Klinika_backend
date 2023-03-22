using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class LekQuery : IRequest<object>
    {
        public string ID_lek { get; set; }
    }

    public class LekQueryHandler : IRequestHandler<LekQuery, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public LekQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<object> Handle(LekQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_lek);

            var t = context.Leks.Where(x => x.IdLek.Equals(id)).First();

            if (!context.LekWMagazynies.Where(x => x.IdLek.Equals(id)).Any())
            {
                return new {
                    Nazwa = t.Nazwa,
                    JednostkaMiary = t.JednostkaMiary,
                    Producent = t.Producent,
                    Choroby = Enumerable.Empty<object>().ToList(),
                    LekList = Enumerable.Empty<GetStanLekuResponse>().ToList()
                };
            }

            return new
            {
                Nazwa = t.Nazwa,
                JednostkaMiary = t.JednostkaMiary,
                Producent = t.Producent,
                Choroby = (from i in context.ChorobaLeks
                           join j in context.Chorobas on i.IdChoroba equals j.IdChoroba into qs
                           from j in qs.DefaultIfEmpty()
                           where i.IdLek == id
                           select new
                           {
                               IdChoroba = hash.Encode(j.IdChoroba),
                               Nazwa = j.Nazwa,
                               NazwaLacinska = j.NazwaLacinska
                           }).ToList(),
                LekList = (from y in context.LekWMagazynies
                           where y.IdLek == id
                           select new GetStanLekuResponse()
                           {
                               IdStanLeku = hash.Encode(y.IdStanLeku),
                               Ilosc = y.Ilosc,
                               DataWaznosci = y.DataWaznosci,
                           }).ToList()
            };
        }
    }
}