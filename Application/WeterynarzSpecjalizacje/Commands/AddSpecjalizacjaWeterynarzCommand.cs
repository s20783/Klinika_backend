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

    public class AddSpecjalizacjaWeterynarzCommandHandler : IRequestHandler<AddSpecjalizacjaWeterynarzCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public AddSpecjalizacjaWeterynarzCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(AddSpecjalizacjaWeterynarzCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = _hash.Decode(req.ID_specjalizacja, req.ID_weterynarz);

            _context.WeterynarzSpecjalizacjas.Add(
                new WeterynarzSpecjalizacja
                {
                    IdSpecjalizacja = id1,
                    IdOsoba = id2
                });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}