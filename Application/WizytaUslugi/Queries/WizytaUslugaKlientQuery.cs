using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Queries
{
    public class WizytaUslugaKlientQuery : IRequest<List<GetUslugaResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class WizytaUslugaKlientQueryHandler : IRequestHandler<WizytaUslugaKlientQuery, List<GetUslugaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaUslugaKlientQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetUslugaResponse>> Handle(WizytaUslugaKlientQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            return (from x in context.Uslugas
                    join y in context.WizytaUslugas on x.IdUsluga equals y.IdUsluga
                    orderby x.NazwaUslugi
                    where y.IdWizyta == id
                    select new GetUslugaResponse()
                    {
                        ID_Usluga = hash.Encode(x.IdUsluga),
                        NazwaUslugi = x.NazwaUslugi,
                        Opis = x.Opis,
                        Narkoza = x.Narkoza,
                        Dolegliwosc = x.Dolegliwosc
                    }).AsParallel().WithCancellation(cancellationToken).ToList();
        }
    }
}