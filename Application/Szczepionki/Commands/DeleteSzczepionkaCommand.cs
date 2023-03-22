using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepionki.Commands
{
    public class DeleteSzczepionkaCommand : IRequest<int>
    {
        public string ID_szczepionka { get; set; }
    }

    public class DeleteSzczepionkaCommandHandler : IRequestHandler<DeleteSzczepionkaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteSzczepionkaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteSzczepionkaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_szczepionka);

            context.LekWMagazynies.RemoveRange(context.LekWMagazynies.Where(x => x.IdLek.Equals(id)).ToList());
            context.Szczepienies.RemoveRange(context.Szczepienies.Where(x => x.IdLek.Equals(id)).ToList());
            context.Szczepionkas.Remove(context.Szczepionkas.Where(x => x.IdLek.Equals(id)).First());
            context.Leks.Remove(context.Leks.Where(x => x.IdLek.Equals(id)).First());

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}