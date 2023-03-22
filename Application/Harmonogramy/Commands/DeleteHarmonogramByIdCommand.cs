using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class DeleteHarmonogramByIdCommand : IRequest<object>
    {
        public string ID_osoba { get; set; }
        public DateTime Data { get; set; }
    }

    public class DeleteHarmonogramByIdCommandHandler : IRequestHandler<DeleteHarmonogramByIdCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public DeleteHarmonogramByIdCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<object> Handle(DeleteHarmonogramByIdCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var harmonograms = context.Harmonograms.Include(x => x.IdWizytaNavigation).Where(x => x.DataRozpoczecia.Date.Equals(req.Data) && x.WeterynarzIdOsoba == id).ToList();

            if (!harmonograms.Any())
            {
                throw new Exception("Harmonogram nie istnieje w dniu: " + req.Data);
            }

            await harmonogramService.DeleteHarmonograms(harmonograms, context);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}