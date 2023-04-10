using Application.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class AutoCreateHarmonogramCommand : IRequest<int>
    {

    }

    public class AutoCreateHarmonogramCommandHandler : IRequestHandler<AutoCreateHarmonogramCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IHarmonogram _harmonogramService;
        public AutoCreateHarmonogramCommandHandler(IKlinikaContext klinikaContext, IHash hash, IHarmonogram harmonogramRepository)
        {
            _context = klinikaContext;
            _hash = hash;
            _harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(AutoCreateHarmonogramCommand req, CancellationToken cancellationToken)
        {
            var endOfCurrentHarmonograms = DateTime.Now.Date;
            if (_context.Harmonograms.Any())
            {
                if (_context.Harmonograms.Max(x => x.DataZakonczenia.Date) > endOfCurrentHarmonograms)
                {
                    endOfCurrentHarmonograms = _context.Harmonograms.Max(x => x.DataZakonczenia.Date);
                }
            }

            var vets = _context.Weterynarzs.ToList();

            for (int i = 1; i <= 7 + 7 - (int)endOfCurrentHarmonograms.DayOfWeek; i++)
            {
                foreach (Weterynarz w in vets)
                {
                    _harmonogramService.CreateWeterynarzHarmonograms(_context, endOfCurrentHarmonograms.AddDays(i), w.IdOsoba);
                }
            }

            return await _context.SaveChangesAsync(cancellationToken);            
        }
    }
}