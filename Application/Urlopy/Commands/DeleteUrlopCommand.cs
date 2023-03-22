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
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public DeleteUrlopCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(DeleteUrlopCommand req, CancellationToken cancellationToken)
        {
            int urlopID = hash.Decode(req.ID_urlop);

            var urlop = context.Urlops.Where(x => x.IdUrlop.Equals(urlopID)).First();
            //harmonogramService.CreateWeterynarzHarmonograms(context, urlop.Dzien, urlop.IdOsoba);
            context.Urlops.Remove(urlop);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}