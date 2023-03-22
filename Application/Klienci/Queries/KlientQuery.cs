using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Queries
{
    public class KlientQuery : IRequest<GetKlientResponse>
    {
        public string ID_osoba { get; set; }
    }

    public class GetKlientQueryHandle : IRequestHandler<KlientQuery, GetKlientResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public GetKlientQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetKlientResponse> Handle(KlientQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var result =
                from x in context.Osobas
                join y in context.Klients on x.IdOsoba equals y.IdOsoba into ps
                from p in ps
                where x.IdOsoba == id
                select new GetKlientResponse()
                {
                    Imie = x.Imie,
                    Nazwisko = x.Nazwisko,
                    NumerTelefonu = x.NumerTelefonu,
                    Email = x.Email,
                    DataZalozeniaKonta = p.DataZalozeniaKonta
                };

            return result.FirstOrDefault();
        }
    }
}