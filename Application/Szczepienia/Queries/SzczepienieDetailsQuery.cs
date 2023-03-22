using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Queries
{
    public class SzczepienieDetailsQuery : IRequest<GetSzczepienieResponse>
    {
        public string ID_szczepienie { get; set; }
    }

    public class SzczepienieDetailsQueryHandler : IRequestHandler<SzczepienieDetailsQuery, GetSzczepienieResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SzczepienieDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetSzczepienieResponse> Handle(SzczepienieDetailsQuery req, CancellationToken cancellationToken)
        { 
            int id = hash.Decode(req.ID_szczepienie);

            return (from x in context.Szczepienies
                    join y in context.Szczepionkas on x.IdLek equals y.IdLek
                    join z in context.Leks on y.IdLek equals z.IdLek
                    where x.IdSzczepienie == id
                    orderby z.Nazwa
                    select new GetSzczepienieResponse()
                    {
                        IdSzczepienie = req.ID_szczepienie,
                        IdPacjent = hash.Encode(x.IdPacjent),
                        IdLek = hash.Encode(x.IdLek),
                        Nazwa = z.Nazwa,
                        Data = x.Data,
                        DataWaznosci = y.OkresWaznosci != null ? x.Data.AddTicks((long)y.OkresWaznosci) : null,
                        Dawka = x.Dawka
                    }).First();
        }
    }
}