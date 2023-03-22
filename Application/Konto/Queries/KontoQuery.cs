using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Queries
{
    public class KontoQuery : IRequest<GetKontoResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetKontoQueryHandle : IRequestHandler<KontoQuery, GetKontoResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetKontoQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetKontoResponse> Handle(KontoQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            return context.Osobas.Where(x => x.IdOsoba == id).Select(x => new GetKontoResponse()
            {
                Imie = x.Imie,
                Nazwisko = x.Nazwisko,
                NazwaUzytkownika = x.NazwaUzytkownika,
                NumerTelefonu = x.NumerTelefonu,
                Email = x.Email
            }).FirstOrDefault();
        }
    }
}