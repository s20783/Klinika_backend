using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WeterynarzSpecjalizacje.Commands
{
    public class RemoveVetSpecializationCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
        public string ID_weterynarz { get; set; }
    }

    public class RemoveSpecjalizacjaWeterynarzCommandHandler : IRequestHandler<RemoveVetSpecializationCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public RemoveSpecjalizacjaWeterynarzCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(RemoveVetSpecializationCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = _hash.Decode(req.ID_specjalizacja, req.ID_weterynarz);

            _context.WeterynarzSpecjalizacjas.Remove(
                _context.WeterynarzSpecjalizacjas.Where(x => x.IdSpecjalizacja == id1 && x.IdOsoba == id2).First()
                );

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}