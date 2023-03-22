using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Queries
{
    public class UslugaWizytaListQuery : IRequest<List<GetUslugaResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class UslugaWizytaListQueryHandler : IRequestHandler<UslugaWizytaListQuery, List<GetUslugaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UslugaWizytaListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetUslugaResponse>> Handle(UslugaWizytaListQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            return (from x in context.WizytaUslugas
                    join y in context.Uslugas on x.IdUsluga equals y.IdUsluga
                    where x.IdWizyta == id
                    select new GetUslugaResponse()
                    {
                        ID_Usluga = hash.Encode(x.IdUsluga),
                        NazwaUslugi = y.NazwaUslugi,
                        Opis = y.Opis,
                        Cena = y.Cena,
                        Narkoza = y.Narkoza,
                        Dolegliwosc = y.Dolegliwosc
                    }).ToList();
        }
    }
}