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
    public class SzczepieniePacjentQuery : IRequest<List<GetSzczepienieResponse>>
    {
        public string ID_pacjent { get; set; }
    }

    public class SzczepieniePacjentQueryHandler : IRequestHandler<SzczepieniePacjentQuery, List<GetSzczepienieResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SzczepieniePacjentQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetSzczepienieResponse>> Handle(SzczepieniePacjentQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_pacjent);

            return (from x in context.Szczepienies
                    join y in context.Szczepionkas on x.IdLek equals y.IdLek
                    join z in context.Leks on y.IdLek equals z.IdLek
                    where x.IdPacjent == id
                    orderby z.Nazwa
                    select new GetSzczepienieResponse()
                    {
                        IdSzczepienie = hash.Encode(x.IdSzczepienie),
                        IdPacjent = req.ID_pacjent,
                        IdLek = hash.Encode(x.IdLek),
                        Nazwa = z.Nazwa,
                        Data = x.Data,
                        DataWaznosci = y.OkresWaznosci != null ? x.Data.AddTicks((long)y.OkresWaznosci) : null,
                        Zastosowanie = y.Zastosowanie,
                        Dawka = x.Dawka
                    }).ToList();
        }
    }
}