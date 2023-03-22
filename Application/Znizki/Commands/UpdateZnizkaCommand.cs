using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Znizki.Commands
{
    public class UpdateZnizkaCommand : IRequest<int>
    {
        public string ID_znizka { get; set; }
        public string Nazwa { get; set; }
        public decimal Procent { get; set; }
    }

    public class UpdateZnizkaCommandHandler : IRequestHandler<UpdateZnizkaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateZnizkaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateZnizkaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_znizka);

            var znizka = context.Znizkas.Where(x => x.IdZnizka.Equals(id)).First();
            znizka.NazwaZnizki = req.Nazwa;
            znizka.ProcentZnizki = req.Procent;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}