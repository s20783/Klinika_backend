using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class UslugaDetailsQuery : IRequest<GetUslugaResponse>
    {
        public string ID_usluga { get; set; }
    }

    public class UslugaDetailsQueryHandler : IRequestHandler<UslugaDetailsQuery, GetUslugaResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UslugaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetUslugaResponse> Handle(UslugaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_usluga);
            
            return (from x in context.Uslugas
                    orderby x.NazwaUslugi
                    where x.IdUsluga == id
                    select new GetUslugaResponse()
                    {
                        ID_Usluga = req.ID_usluga,
                        NazwaUslugi = x.NazwaUslugi,
                        Opis = x.Opis,
                        Cena = x.Cena,
                        Narkoza = x.Narkoza,
                        Dolegliwosc = x.Dolegliwosc
                    }).First();
        }
    }
}