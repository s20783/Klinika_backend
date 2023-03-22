using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Queries
{
    public class SpecjalizacjaDetailsQuery : IRequest<GetSpecjalizacjaResponse>
    {
        public string ID_specjalizacja { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandle : IRequestHandler<SpecjalizacjaDetailsQuery, GetSpecjalizacjaResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SpecjalizacjaDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetSpecjalizacjaResponse> Handle(SpecjalizacjaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_specjalizacja);

            return (from x in context.Specjalizacjas
                    where x.IdSpecjalizacja == id
                    select new GetSpecjalizacjaResponse()
                    {
                        IdSpecjalizacja = req.ID_specjalizacja,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis
                    }).FirstOrDefault();
        }
    }
}