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
    public class DeleteUrlopCommand : IRequest<int>
    {
        public string ID_urlop { get; set; }
    }

    public class DeleteUrlopCommandHandler : IRequestHandler<DeleteUrlopCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IHarmonogram _harmonogramService;
        public DeleteUrlopCommandHandler(IKlinikaContext klinikaContext, IHash hash, IHarmonogram harmonogram)
        {
            _context = klinikaContext;
            _hash = hash;
            _harmonogramService = harmonogram;
        }

        public async Task<int> Handle(DeleteUrlopCommand req, CancellationToken cancellationToken)
        {
            int urlopID = _hash.Decode(req.ID_urlop);

            var urlop = _context.Urlops.Where(x => x.IdUrlop.Equals(urlopID)).First();
            //harmonogramService.CreateWeterynarzHarmonograms(context, urlop.Dzien, urlop.IdOsoba);
            _context.Urlops.Remove(urlop);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}