using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Queries
{
    public class WeterynarzQuery : IRequest<GetWeterynarzResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetWeterynarzQueryHandle : IRequestHandler<WeterynarzQuery, GetWeterynarzResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetWeterynarzQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetWeterynarzResponse> Handle(WeterynarzQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var results =
                (from x in context.Osobas
                 join y in context.Weterynarzs on x.IdOsoba equals y.IdOsoba into ps
                 from p in ps
                 where x.IdOsoba == id
                 select new GetWeterynarzResponse()
                 {
                     IdOsoba = x.IdOsoba,
                     Imie = x.Imie,
                     Nazwisko = x.Nazwisko,
                     NumerTelefonu = x.NumerTelefonu,
                     Email = x.Email,
                     DataZatrudnienia = p.DataZatrudnienia,
                     DataUrodzenia = p.DataUrodzenia,
                     Pensja = p.Pensja
                 }).FirstOrDefault();

            return results;
        }
    }
}