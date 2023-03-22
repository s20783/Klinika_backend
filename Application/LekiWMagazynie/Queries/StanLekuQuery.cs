using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Queries
{
    public class StanLekuQuery : IRequest<GetStanLekuResponse>
    {
        public string ID_stan_leku { get; set; }
    }

    public class GetStanLekuQueryHandle : IRequestHandler<StanLekuQuery, GetStanLekuResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetStanLekuQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetStanLekuResponse> Handle(StanLekuQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_stan_leku);

            return (from p in context.LekWMagazynies
                    where p.IdStanLeku == id
                    select new GetStanLekuResponse()
                    {
                        Ilosc = p.Ilosc,
                        DataWaznosci = p.DataWaznosci
                    }).FirstOrDefault();
        }
    }
}