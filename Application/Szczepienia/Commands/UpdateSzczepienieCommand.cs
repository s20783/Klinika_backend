using Application.DTO.Requests;
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
    public class UpdateSzczepienieCommand : IRequest<int>
    {
        public string ID_szczepienie { get; set; }
        public SzczepienieRequest request { get; set; }
    }

    public class UpdateSzczepienieCommandHandler : IRequestHandler<UpdateSzczepienieCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateSzczepienieCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateSzczepienieCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_szczepienie);

            var szczepienie = context.Szczepienies.Where(x => x.IdSzczepienie.Equals(id)).First();
            szczepienie.IdLek = hash.Decode(req.request.IdLek);
            szczepienie.IdPacjent = hash.Decode(req.request.IdPacjent);
            szczepienie.Data = req.request.Data;
            szczepienie.Dawka = req.request.Dawka;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}