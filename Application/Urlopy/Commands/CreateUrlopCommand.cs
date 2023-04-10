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
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IHarmonogram _harmonogramService;
        public CreateUrlopCommandHandler(IKlinikaContext klinikaContext, IHash hash, IHarmonogram harmonogram)
        {
            _context = klinikaContext;
            _hash = hash;
            _harmonogramService = harmonogram;
        }

        public async Task<int> Handle(CreateUrlopCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.request.ID_weterynarz);

            if(_context.Urlops.Where(x => x.Dzien.Date.Equals(req.request.Dzien) && x.IdOsoba.Equals(id)).Any())
            {
                throw new Exception("Taki urlop już istnieje");
            }

            _context.Urlops.Add(new Domain.Models.Urlop
            {
                IdOsoba = id,
                Dzien = req.request.Dzien.Date
            });

            var harmonograms = _context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.request.Dzien.Date) && x.WeterynarzIdOsoba.Equals(id)).ToList();
            await _harmonogramService.DeleteHarmonograms(harmonograms, _context);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}