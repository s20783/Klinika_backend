using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class AutoUpdateHarmonogramCommand : IRequest<int>
    {

    }

    public class UpdateHarmonogramCommandHandler : IRequestHandler<AutoUpdateHarmonogramCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IHarmonogram _harmonogramService;
        public UpdateHarmonogramCommandHandler(IKlinikaContext klinikaContext, IHash hash, IHarmonogram harmonogramRepository)
        {
            _context = klinikaContext;
            _hash = hash;
            _harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(AutoUpdateHarmonogramCommand req, CancellationToken cancellationToken)
        {
            var startDate = DateTime.Now.Date;
            var endOfCurrentHarmonograms = DateTime.Now.Date;

            if (_context.Harmonograms.Any())
            {
                if (_context.Harmonograms.Max(x => x.DataZakonczenia.Date) > endOfCurrentHarmonograms)
                {
                    endOfCurrentHarmonograms = _context.Harmonograms.Max(x => x.DataZakonczenia.Date);
                }
            }
            
            if (endOfCurrentHarmonograms > startDate)
            {
                var vets = _context.Weterynarzs.ToList();
                var diff = (endOfCurrentHarmonograms - startDate).Days;
                foreach(Weterynarz w in vets)
                {
                    for (int i = 0; i <= diff; i++)
                    {
                        if (!_context.Harmonograms.Where(x => x.WeterynarzIdOsoba == w.IdOsoba && x.DataRozpoczecia.Date == startDate.AddDays(i)).Any())
                        {
                            _harmonogramService.CreateWeterynarzHarmonograms(_context, startDate.AddDays(i), w.IdOsoba);
                        }
                    }
                }
                return await _context.SaveChangesAsync(cancellationToken);
            }
            return 0;
        }
    }
}