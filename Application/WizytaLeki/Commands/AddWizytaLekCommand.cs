using Application.DTO.Responses;
using Application.Interfaces;
using Application.WizytaLeki.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaLeki.Commands
{
    public class AddWizytaLekCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_Lek { get; set; }
        public int Ilosc { get; set; }
    }

    public class AddWizytaLekCommandHandler : IRequestHandler<AddWizytaLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public AddWizytaLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(AddWizytaLekCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_wizyta, req.ID_Lek);

            context.WizytaLeks.Add(new Domain.Models.WizytaLek
            {
                IdWizyta = id1,
                IdLek = id2,
                Ilosc = req.Ilosc
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}