using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class RemoveSpecjalizacjaWeterynarzCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
        public string ID_weterynarz { get; set; }
    }

    public class RemoveSpecjalizacjaWeterynarzCommandHandle : IRequestHandler<RemoveSpecjalizacjaWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public RemoveSpecjalizacjaWeterynarzCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(RemoveSpecjalizacjaWeterynarzCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_specjalizacja, req.ID_weterynarz);

            context.WeterynarzSpecjalizacjas.Remove(
                context.WeterynarzSpecjalizacjas.Where(x => x.IdSpecjalizacja == id1 && x.IdOsoba == id2).First()
                );

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}