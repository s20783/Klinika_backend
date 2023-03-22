using Application.DTO.Responses;
using Application.Interfaces;
using Application.ReceptaLeki.Queries;
using Application.Recepty.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Queries
{
    public class WizytaChorobaQuery : IRequest<List<GetChorobaResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class WizytaChorobaQueryHandler : IRequestHandler<WizytaChorobaQuery, List<GetChorobaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaChorobaQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetChorobaResponse>> Handle(WizytaChorobaQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            return (from x in context.WizytaChorobas
                    join y in context.Chorobas on x.IdChoroba equals y.IdChoroba
                    where x.IdWizyta == id
                    select new GetChorobaResponse()
                    {
                        ID_Choroba = hash.Encode(x.IdChoroba),
                        Nazwa = y.Nazwa,
                        NazwaLacinska = y.NazwaLacinska,
                        Opis = y.Opis
                    }).ToList();
        }
    }
}