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
    public class CreateReceptaLekCommand : IRequest<int>
    {
        public string ID_Recepta { get; set; }
        public string ID_Lek { get; set; }
        public int Ilosc { get; set; }
    }

    public class CreateReceptaLekCommandHandler : IRequestHandler<CreateReceptaLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateReceptaLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateReceptaLekCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Recepta);
            int id2 = hash.Decode(req.ID_Lek);

            context.ReceptaLeks.Add(new Domain.Models.ReceptaLek
            {
                IdWizyta = id,
                IdLek = id2,
                Ilosc = req.Ilosc,
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}