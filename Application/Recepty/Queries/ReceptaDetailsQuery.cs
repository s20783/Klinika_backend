using Application.DTO.Responses;
using Application.Interfaces;
using Application.ReceptaLeki.Queries;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Queries
{
    public class ReceptaDetailsQuery : IRequest<GetReceptaResponse>
    {
        public string ID_recepta { get; set; }
    }

    public class ReceptaDetailsQueryHandler : IRequestHandler<ReceptaDetailsQuery, GetReceptaResponse>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public ReceptaDetailsQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<GetReceptaResponse> Handle(ReceptaDetailsQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_recepta);

            return await (from x in _context.Recepta
                          join s in _context.Wizyta on x.IdWizyta equals s.IdWizyta
                          join l in _context.ReceptaLeks on x.IdWizyta equals l.IdWizyta into receptaLek
                          from y in receptaLek.DefaultIfEmpty()
                          where s.IdWizyta == id
                          select new GetReceptaResponse()
                          {
                              ID_Recepta = _hash.Encode(x.IdWizyta),
                              Zalecenia = x.Zalecenia,
                              Leki = x.ReceptaLeks.Select(x => new GetReceptaLekResponse
                              {
                                  ID_Lek = _hash.Encode(x.IdLek),
                                  Nazwa = x.IdLekNavigation.Nazwa,
                                  Ilosc = y.Ilosc,
                                  Producent = x.IdLekNavigation.Producent,
                                  JednostkaMiary = x.IdLekNavigation.JednostkaMiary
                              }).ToList(),
                              WizytaData = _context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Any() ? _context.Harmonograms.Where(x => x.IdWizyta.Equals(x.IdWizyta)).Min(y => y.DataRozpoczecia) : null
                          }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}