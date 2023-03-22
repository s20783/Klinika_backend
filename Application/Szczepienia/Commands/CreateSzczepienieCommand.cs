using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Commands
{
    public class CreateSzczepienieCommand : IRequest<int>
    {
        public SzczepienieRequest request { get; set; }
    }

    public class CreateSzczepienieCommandHandler : IRequestHandler<CreateSzczepienieCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateSzczepienieCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateSzczepienieCommand req, CancellationToken cancellationToken)
        {
            int idLek = hash.Decode(req.request.IdLek);
            int idPacjent = hash.Decode(req.request.IdPacjent);

            context.Szczepienies.Add(new Szczepienie
            {
                IdLek = idLek,
                IdPacjent = idPacjent,
                Data = req.request.Data,
                Dawka = req.request.Dawka
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}