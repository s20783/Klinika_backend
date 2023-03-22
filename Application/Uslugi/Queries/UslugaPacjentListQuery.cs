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
    public class UslugaPacjentListQuery : IRequest<List<GetUslugaPacjentResponse>>
    {
        public string ID_pacjent { get; set; }
    }

    public class UslugaPacjentListQueryHandler : IRequestHandler<UslugaPacjentListQuery, List<GetUslugaPacjentResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UslugaPacjentListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetUslugaPacjentResponse>> Handle(UslugaPacjentListQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_pacjent);

            return (from x in context.WizytaUslugas
                    join y in context.Uslugas on x.IdUsluga equals y.IdUsluga
                    join w in context.Wizyta on x.IdWizyta equals w.IdWizyta
                    where w.IdPacjent == id
                    select new GetUslugaPacjentResponse()
                    {
                        ID_Usluga = hash.Encode(x.IdUsluga),
                        ID_wizyta = hash.Encode(w.IdWizyta),
                        NazwaUslugi = y.NazwaUslugi,
                        Opis = y.Opis,
                        Data = w.Harmonograms.Min(x => x.DataRozpoczecia)
                    }).ToList();
        }
    }
}