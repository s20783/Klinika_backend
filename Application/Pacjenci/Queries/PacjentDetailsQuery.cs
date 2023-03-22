using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PacjentDetailsQuery : IRequest<GetPacjentDetailsResponse>
    {
        public string ID_pacjent { get; set; }
    }

    public class PacjentDetailsQueryHandle : IRequestHandler<PacjentDetailsQuery, GetPacjentDetailsResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public PacjentDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetPacjentDetailsResponse> Handle(PacjentDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_pacjent);

            var result =
                (from x in context.Pacjents
                 join y in context.Osobas on x.IdOsoba equals y.IdOsoba
                 where x.IdPacjent == id
                 orderby x.Nazwa
                 select new GetPacjentDetailsResponse()
                 {
                     IdOsoba = hash.Encode(x.IdOsoba),
                     Nazwa = x.Nazwa,
                     Gatunek = x.Gatunek,
                     Rasa = x.Rasa,
                     Masc = x.Masc,
                     Plec = x.Plec,
                     DataUrodzenia = x.DataUrodzenia,
                     Waga = x.Waga,
                     Agresywne = x.Agresywne,
                     Wlasciciel = y.Imie + " " + y.Nazwisko,
                     Ubezplodnienie = x.Ubezplodnienie
                 }).First();

            return result;
        }
    }
}