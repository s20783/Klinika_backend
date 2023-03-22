using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Commands
{
    public class RemoveWizytaChorobaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_choroba { get; set; }
    }

    public class RemoveWizytaChorobaCommandHandler : IRequestHandler<RemoveWizytaChorobaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public RemoveWizytaChorobaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(RemoveWizytaChorobaCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_wizyta, req.ID_choroba);

            context.WizytaChorobas.Remove(context.WizytaChorobas.First(x => x.IdWizyta == id1 && x.IdChoroba == id2));
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
