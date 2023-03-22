using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ReceptaLeki.Commands
{
    public class DeleteReceptaLekCommand : IRequest<int>
    {
        public string ID_Recepta { get; set; }
        public string ID_Lek { get; set; }
    }

    public class DeleteReceptaLekCommandHandler : IRequestHandler<DeleteReceptaLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteReceptaLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteReceptaLekCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Recepta);
            int id2 = hash.Decode(req.ID_Lek);

            var receptaLek = context.ReceptaLeks.Where(x => x.IdWizyta.Equals(id) && x.IdLek.Equals(id2)).First();
            context.ReceptaLeks.Remove(receptaLek);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}