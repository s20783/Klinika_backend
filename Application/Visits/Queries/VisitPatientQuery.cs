using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class VisitPatientQuery : IRequest<List<GetVisitListResponse>>
    {
        public string ID_Pacjent { get; set; }
    }

    public class WizytaPacjentQueryHandler : IRequestHandler<VisitPatientQuery, List<GetVisitListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public WizytaPacjentQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<List<GetVisitListResponse>> Handle(VisitPatientQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Pacjent);

            return (from x in _context.Wizyta
                    join d in _context.Harmonograms on x.IdWizyta equals d.IdWizyta into harmonogram
                    from y in harmonogram.DefaultIfEmpty()
                    where x.IdPacjent == id
                    group x by new { x.IdWizyta, x.IdOsoba, x.IdPacjent, x.Status, x.CzyOplacona, y.WeterynarzIdOsoba }
                    into g
                    select new GetVisitListResponse()
                    {
                        IdWizyta = _hash.Encode(g.Key.IdWizyta),
                        IdPacjent = req.ID_Pacjent,
                        Pacjent = null,
                        IdKlient = null,
                        Klient = null,
                        IdWeterynarz = g.Key.WeterynarzIdOsoba != null ? _hash.Encode(g.Key.WeterynarzIdOsoba) : null,
                        Weterynarz = g.Key.WeterynarzIdOsoba != null ? _context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                        Status = g.Key.Status,
                        CzyOplacona = g.Key.CzyOplacona,
                        Data = g.Key.WeterynarzIdOsoba != null ? _context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).OrderBy(x => x.DataRozpoczecia).Select(x => x.DataRozpoczecia).First() : null
                    })
                    .ToList()
                    .OrderByDescending(x => x.Data)
                    .ToList();
        }
    }
}