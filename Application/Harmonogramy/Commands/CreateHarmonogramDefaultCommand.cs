using Application.Interfaces;
using Domain;
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
    public class CreateHarmonogramDefaultCommand : IRequest<object>
    {
        public DateTime Data { get; set; }
    }

    public class CreateHarmonogramDefaultCommandHandler : IRequestHandler<CreateHarmonogramDefaultCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public CreateHarmonogramDefaultCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<object> Handle(CreateHarmonogramDefaultCommand req, CancellationToken cancellationToken)
        {
            var weterynarze = context.Weterynarzs.ToList();

            foreach (Weterynarz w in weterynarze)
            {
                if (!context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.Data) && x.WeterynarzIdOsoba == w.IdOsoba).Any())
                {
                    harmonogramService.CreateWeterynarzHarmonograms(context, req.Data, w.IdOsoba);
                }
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}