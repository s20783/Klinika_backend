using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Urlopy.Commands
{
    public class UpdateUrlopCommand : IRequest<int>
    {
        public string ID_urlop { get; set; }
        public UrlopRequest request { get; set; }
    }

    public class UpdateUrlopCommandHandler : IRequestHandler<UpdateUrlopCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public UpdateUrlopCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(UpdateUrlopCommand req, CancellationToken cancellationToken)
        {
            int weterynarzID = hash.Decode(req.request.ID_weterynarz);
            int urlopID = hash.Decode(req.ID_urlop);

            var urlop = context.Urlops.Where(x => x.IdUrlop.Equals(urlopID)).First();

            await harmonogramService.DeleteHarmonograms(
                context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.request.Dzien) && x.WeterynarzIdOsoba.Equals(weterynarzID)).ToList(),
                context);

            urlop.IdOsoba = hash.Decode(req.request.ID_weterynarz);
            urlop.Dzien = req.request.Dzien.Date;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}