using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChorobaLeki.Commands
{
    public class AddChorobaLekCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public string ID_choroba { get; set; }
    }

    public class AddChorobaLekCommandHandler : IRequestHandler<AddChorobaLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public AddChorobaLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(AddChorobaLekCommand req, CancellationToken cancellationToken)
        {
            int lekID = hash.Decode(req.ID_lek);
            int chorobaID = hash.Decode(req.ID_choroba);

            context.ChorobaLeks.Add(new ChorobaLek
            {
                IdChoroba = chorobaID,
                IdLek = lekID
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}