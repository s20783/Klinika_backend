using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Commands
{
    public class DeleteReceptaCommand : IRequest<int>
    {
        public string ID_recepta { get; set; }
    }

    public class DeleteReceptaCommandHandler : IRequestHandler<DeleteReceptaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteReceptaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_recepta);

            context.ReceptaLeks.RemoveRange(context.ReceptaLeks.Where(x => x.IdWizyta.Equals(id)).ToList());
            context.Recepta.Remove(context.Recepta.Where(x => x.IdWizyta.Equals(id)).First());
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}