using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Queries
{
    public class PacjentKlientListQuery : IRequest<List<GetPacjentKlientListResponse>>
    {
        public string ID_osoba { get; set; }
    }

    public class PacjentKlientListQueryHandle : IRequestHandler<PacjentKlientListQuery, List<GetPacjentKlientListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public PacjentKlientListQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetPacjentKlientListResponse>> Handle(PacjentKlientListQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            if (context.Klients.Where(x => x.IdOsoba == id).Any() != true)
            {
                throw new Exception("Nie ma klienta o ID = " + req.ID_osoba);
            }

            var results =
            (from x in context.Pacjents
             where x.IdOsoba == id
             orderby x.Nazwa
             select new GetPacjentKlientListResponse()
             {
                 IdPacjent = hash.Encode(x.IdPacjent),
                 Nazwa = x.Nazwa,
                 Gatunek = x.Gatunek,
                 Rasa = x.Rasa,
                 Plec = x.Plec,
                 Agresywne = x.Agresywne
             }).AsParallel().WithCancellation(cancellationToken).ToList();

            return results;
        }
    }
}