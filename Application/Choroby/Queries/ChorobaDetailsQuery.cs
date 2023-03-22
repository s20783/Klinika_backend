using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Queries
{
    public class ChorobaDetailsQuery : IRequest<GetChorobaResponse>
    {
        public string ID_Choroba { get; set; }
    }

    public class ChorobaDetailsQueryHandler : IRequestHandler<ChorobaDetailsQuery, GetChorobaResponse>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public ChorobaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<GetChorobaResponse> Handle(ChorobaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Choroba);

            return (from x in context.Chorobas
                    where x.IdChoroba == id
                    select new GetChorobaResponse()
                    {
                        ID_Choroba = req.ID_Choroba,
                        Nazwa = x.Nazwa,
                        NazwaLacinska = x.NazwaLacinska,
                        Opis = x.Opis
                    }).First();
        }
    }
}