using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Commands
{
    public class DeleteUslugaCommand : IRequest<int>
    {
        public string ID_usluga { get; set; }
    }

    public class DeleteUslugaCommandHandler : IRequestHandler<DeleteUslugaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetUslugaResponse> cache;
        public DeleteUslugaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetUslugaResponse> _cache)
        {
            context = klinikaContext;
            hash = ihash;
            cache = _cache;
        }

        public async Task<int> Handle(DeleteUslugaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_usluga);

            context.WizytaUslugas.RemoveRange(context.WizytaUslugas.Where(x => x.IdUsluga.Equals(id)).ToList());
            context.Uslugas.Remove(context.Uslugas.Where(x => x.IdUsluga.Equals(id)).First());

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}