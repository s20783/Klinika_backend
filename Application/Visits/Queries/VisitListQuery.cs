using Application.DTO.Responses;
using Application.Interfaces;
using Domain;
using MediatR;
using ServiceLayer.DTO.Responses;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Queries
{
    public class VisitListQuery : IRequest<PaginatedResponse<GetVisitListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class WizytaListQueryHandler : IRequestHandler<VisitListQuery, PaginatedResponse<GetVisitListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public WizytaListQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<PaginatedResponse<GetVisitListResponse>> Handle(VisitListQuery req, CancellationToken cancellationToken)
        {
            var data =
                (from x in _context.Wizyta
                 join d in _context.Harmonograms on x.IdWizyta equals d.IdWizyta into harmonogram
                 from y in harmonogram.DefaultIfEmpty()
                 join k in _context.Osobas on x.IdOsoba equals k.IdOsoba
                 join d in _context.Pacjents on x.IdPacjent equals d.IdPacjent into pacjent
                 from p in pacjent.DefaultIfEmpty()
                 group x by new { x.IdWizyta, x.IdOsoba, x.IdPacjent, x.Status, x.CzyOplacona, p.Nazwa, y.WeterynarzIdOsoba, k.Imie, k.Nazwisko }
                        into g
                 select new GetVisitListResponse()
                 {
                     IdWizyta = _hash.Encode(g.Key.IdWizyta),
                     IdPacjent = g.Key.IdPacjent != null ? _hash.Encode((int)g.Key.IdPacjent) : null,
                     Pacjent = g.Key.IdPacjent != null ? g.Key.Nazwa : null,
                     IdKlient = g.Key.IdOsoba != null ? _hash.Encode((int)g.Key.IdOsoba) : "",
                     Klient = g.Key.Imie + " " + g.Key.Nazwisko,
                     IdWeterynarz = g.Key.WeterynarzIdOsoba != null ? _hash.Encode(g.Key.WeterynarzIdOsoba) : null,
                     Weterynarz = g.Key.WeterynarzIdOsoba != null ? _context.Osobas.Where(i => i.IdOsoba.Equals(g.Key.WeterynarzIdOsoba)).Select(i => i.Imie + " " + i.Nazwisko).First() : null,
                     Status = g.Key.Status,
                     CzyOplacona = g.Key.CzyOplacona,
                     Data = g.Key.WeterynarzIdOsoba != null ? _context.Harmonograms.Where(x => x.IdWizyta.Equals(g.Key.IdWizyta)).OrderBy(x => x.DataRozpoczecia).Select(x => x.DataRozpoczecia).First() : null
                 })
                 .AsParallel()
                 .WithCancellation(cancellationToken);


            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Pacjent.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Klient.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Weterynarz.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderByDescending(x => x.Data);


            return new PaginatedResponse<GetVisitListResponse>
            {
                Items = results
                    .Skip((req.Page - 1) * GlobalValues.PAGE_SIZE)
                    .Take(GlobalValues.PAGE_SIZE)
                    .ToList(),
                PageCount = (int)Math.Ceiling(results.Count() / (double)GlobalValues.PAGE_SIZE),
                PageIndex = req.Page
            };
        }
    }
}