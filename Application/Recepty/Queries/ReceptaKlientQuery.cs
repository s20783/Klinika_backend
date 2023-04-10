using Application.DTO.Responses;
using Application.Interfaces;
using Application.ReceptaLeki.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Queries
{
    public class ReceptaKlientQuery : IRequest<List<GetReceptaResponse>>
    {
        public string ID_klient { get; set; }
    }

    public class ReceptaKlientQueryHandler : IRequestHandler<ReceptaKlientQuery, List<GetReceptaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public ReceptaKlientQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<List<GetReceptaResponse>> Handle(ReceptaKlientQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_klient);

            return await (from x in _context.Recepta
                          join s in _context.Wizyta on x.IdWizyta equals s.IdWizyta
                          join l in _context.ReceptaLeks on x.IdWizyta equals l.IdWizyta into receptaLek
                          from y in receptaLek.DefaultIfEmpty()
                          join z in _context.Pacjents on s.IdPacjent equals z.IdPacjent into pacjent
                          from p in pacjent.DefaultIfEmpty()
                          where s.IdOsoba == id
                          select new GetReceptaResponse()
                          {
                              ID_Recepta = _hash.Encode(x.IdWizyta),
                              Pacjent = s.IdPacjent != null ? p.Nazwa : null,
                              Zalecenia = x.Zalecenia,
                              Leki = x.ReceptaLeks.Select(x => new GetReceptaLekResponse
                              {
                                  ID_Lek = _hash.Encode(x.IdLek),
                                  Nazwa = x.IdLekNavigation.Nazwa,
                                  Ilosc = y.Ilosc,
                                  Producent = x.IdLekNavigation.Producent,
                                  JednostkaMiary = x.IdLekNavigation.JednostkaMiary
                              }).ToList(),
                              WizytaData = _context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Any() ? _context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Min(y => y.DataRozpoczecia) : null,
                          }).ToListAsync(cancellationToken);
        }
    }
}