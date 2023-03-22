using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Urlopy.Commands
{
    public class CreateUrlopCommand : IRequest<int>
    {
        public UrlopRequest request { get; set; }
    }

    public class CreateUrlopCommandHandler : IRequestHandler<CreateUrlopCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public CreateUrlopCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(CreateUrlopCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.request.ID_weterynarz);

            if(context.Urlops.Where(x => x.Dzien.Date.Equals(req.request.Dzien) && x.IdOsoba.Equals(id)).Any())
            {
                throw new Exception("Taki urlop już istnieje");
            }

            context.Urlops.Add(new Domain.Models.Urlop
            {
                IdOsoba = id,
                Dzien = req.request.Dzien.Date
            });

            var harmonograms = context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.request.Dzien.Date) && x.WeterynarzIdOsoba.Equals(id)).ToList();
            await harmonogramService.DeleteHarmonograms(harmonograms, context);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}