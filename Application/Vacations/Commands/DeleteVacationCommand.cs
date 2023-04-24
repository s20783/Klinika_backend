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
    public class DeleteVacationCommand : IRequest<int>
    {
        public string ID_urlop { get; set; }
    }

    public class DeleteUrlopCommandHandler : IRequestHandler<DeleteVacationCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ISchedule _harmonogramService;
        public DeleteUrlopCommandHandler(IKlinikaContext klinikaContext, IHash hash, ISchedule harmonogram)
        {
            _context = klinikaContext;
            _hash = hash;
            _harmonogramService = harmonogram;
        }

        public async Task<int> Handle(DeleteVacationCommand req, CancellationToken cancellationToken)
        {
            int urlopID = _hash.Decode(req.ID_urlop);

            var urlop = _context.Urlops.Where(x => x.IdUrlop.Equals(urlopID)).First();
            //harmonogramService.CreateWeterynarzHarmonograms(context, urlop.Dzien, urlop.IdOsoba);
            _context.Urlops.Remove(urlop);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}