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
    public class UpdateHarmonogramCommand : IRequest<int>
    {

    }

    public class UpdateHarmonogramCommandHandler : IRequestHandler<UpdateHarmonogramCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public UpdateHarmonogramCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(UpdateHarmonogramCommand req, CancellationToken cancellationToken)
        {
            var startDate = DateTime.Now.Date;

            var endOfCurrentHarmonograms = context.Harmonograms.Max(x => x.DataZakonczenia).Date;
            if (endOfCurrentHarmonograms > startDate)
            {
                var vets = context.Weterynarzs.ToList();
                var diff = (endOfCurrentHarmonograms - startDate).Days;
                foreach(Weterynarz w in vets)
                {
                    for (int i = 0; i <= diff; i++)
                    {
                        if (!context.Harmonograms.Where(x => x.WeterynarzIdOsoba == w.IdOsoba && x.DataRozpoczecia.Date == startDate.AddDays(i)).Any())
                        {
                            harmonogramService.CreateWeterynarzHarmonograms(context, startDate.AddDays(i), w.IdOsoba);
                        }
                    }
                }

                return await context.SaveChangesAsync(cancellationToken);
            }

            return 0;
        }
    }
}