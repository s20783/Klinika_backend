using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class AddSpecjalizacjaWeterynarzCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
        public string ID_weterynarz { get; set; }
    }

    public class AddSpecjalizacjaWeterynarzCommandHandle : IRequestHandler<AddSpecjalizacjaWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public AddSpecjalizacjaWeterynarzCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(AddSpecjalizacjaWeterynarzCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_specjalizacja, req.ID_weterynarz);

            context.WeterynarzSpecjalizacjas.Add(
                new WeterynarzSpecjalizacja
                {
                    IdSpecjalizacja = id1,
                    IdOsoba = id2
                });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}