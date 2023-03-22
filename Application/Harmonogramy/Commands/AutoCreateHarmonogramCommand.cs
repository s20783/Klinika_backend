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
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public AutoCreateHarmonogramCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(AutoCreateHarmonogramCommand req, CancellationToken cancellationToken)
        {
            var endOfCurrentHarmonograms = context.Harmonograms.Max(x => x.DataZakonczenia).Date;
            if (endOfCurrentHarmonograms < DateTime.Now)
            {
                endOfCurrentHarmonograms = DateTime.Now.Date;
            }

            var vets = context.Weterynarzs.ToList();

            for (int i = 1; i <= 7 + 7 - (int)endOfCurrentHarmonograms.DayOfWeek; i++)
            {
                foreach (Weterynarz w in vets)
                {
                    harmonogramService.CreateWeterynarzHarmonograms(context, endOfCurrentHarmonograms.AddDays(i), w.IdOsoba);
                }
            }

            return await context.SaveChangesAsync(cancellationToken);            
        }
    }
}