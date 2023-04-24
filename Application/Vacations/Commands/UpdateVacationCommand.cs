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
    public class UpdateVacationCommand : IRequest<int>
    {
        public string ID_urlop { get; set; }
        public VacationRequest request { get; set; }
    }

    public class UpdateUrlopCommandHandler : IRequestHandler<UpdateVacationCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ISchedule _harmonogramService;
        public UpdateUrlopCommandHandler(IKlinikaContext klinikaContext, IHash hash, ISchedule harmonogramRepository)
        {
            _context = klinikaContext;
            _hash = hash;
            _harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(UpdateVacationCommand req, CancellationToken cancellationToken)
        {
            int weterynarzID = _hash.Decode(req.request.ID_weterynarz);
            int urlopID = _hash.Decode(req.ID_urlop);

            var urlop = _context.Urlops.Where(x => x.IdUrlop.Equals(urlopID)).First();

            await _harmonogramService.DeleteSchedules(
                _context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.request.Dzien) && x.WeterynarzIdOsoba.Equals(weterynarzID)).ToList(),
                _context);

            urlop.IdOsoba = _hash.Decode(req.request.ID_weterynarz);
            urlop.Dzien = req.request.Dzien.Date;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}