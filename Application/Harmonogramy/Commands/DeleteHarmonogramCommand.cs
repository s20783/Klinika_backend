using Application.Interfaces;
using Domain.Enums;
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
    public class DeleteHarmonogramCommand : IRequest<object>
    {
        public DateTime Data { get; set; }
    }

    public class DeleteHarmonogramCommandHandler : IRequestHandler<DeleteHarmonogramCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public DeleteHarmonogramCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<object> Handle(DeleteHarmonogramCommand req, CancellationToken cancellationToken)
        {
            var harmonograms = context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.Data)).ToList();

            if (!harmonograms.Any())
            {
                throw new Exception("Harmonogram nie istnieje w dniu: " + req.Data);
            }

            await harmonogramService.DeleteHarmonograms(harmonograms, context);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}