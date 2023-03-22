using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChorobaLeki.Commands
{
    public class RemoveChorobaLekCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public string ID_choroba { get; set; }
    }

    public class RemoveChorobaLekCommandHandler : IRequestHandler<RemoveChorobaLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public RemoveChorobaLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(RemoveChorobaLekCommand req, CancellationToken cancellationToken)
        {
            int lekID = hash.Decode(req.ID_lek);
            int chorobaID = hash.Decode(req.ID_choroba);

            var result = context.ChorobaLeks.Where(x => x.IdLek.Equals(lekID) && x.IdChoroba.Equals(chorobaID)).First();
            context.ChorobaLeks.Remove(result);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
