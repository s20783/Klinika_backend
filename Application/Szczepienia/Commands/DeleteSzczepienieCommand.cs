using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Commands
{
    public class DeleteSzczepienieCommand : IRequest<int>
    {
        public string ID_szczepienie { get; set; }
    }

    public class DeleteSzczepienieCommandHandler : IRequestHandler<DeleteSzczepienieCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteSzczepienieCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteSzczepienieCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_szczepienie);

            var szczepienie = context.Szczepienies.Where(x => x.IdSzczepienie.Equals(id)).First();
            context.Szczepienies.Remove(szczepienie);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}