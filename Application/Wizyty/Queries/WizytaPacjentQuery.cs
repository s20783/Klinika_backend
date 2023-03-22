using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class WizytaPacjentQuery : IRequest<List<GetWizytaListResponse>>
    {
        public string ID_Pacjent { get; set; }
    }

    public class WizytaPacjentQueryHandler : IRequestHandler<WizytaPacjentQuery, List<GetWizytaListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaPacjentQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetWizytaListResponse>> Handle(WizytaPacjentQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Pacjent);

            return (from x in context.Wizyta
                    join d in context.Harmonograms on x.IdWizyta equals d.IdWizyta into harmonogram
                    from y in harmonogram.DefaultIfEmpty()
                    where x.IdPacjent == id
                    group x by new { x.IdWizyta, x.IdOsoba, x.IdPacjent, x.Status, x.CzyOplacona, y.WeterynarzIdOsoba }
                    into g
                    select new GetWizytaListResponse()
                    {
                        IdWizyta = hash.Encode(g.Key.IdWizyta),
                        IdPacjent = req.ID_Pacjent,
                        Pacjent = null,
                        IdKlient = null,
                        Klient = null,
                        IdWeterynarz = g.Key.WeterynarzIdOsoba != null ? hash.Encode(g.Key.WeterynarzIdOsoba) : null,
                        Weterynarz = g.Key.WeterynarzIdOsoba != null ? context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                        Status = g.Key.Status,
                        CzyOplacona = g.Key.CzyOplacona,
                        Data = g.Key.WeterynarzIdOsoba != null ? context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).OrderBy(x => x.DataRozpoczecia).Select(x => x.DataRozpoczecia).First() : null
                    })
                    .AsParallel()
                    .WithCancellation(cancellationToken)
                    .ToList()
                    .OrderByDescending(x => x.Data)
                    .ToList();
        }
    }
}