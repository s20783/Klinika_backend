using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Commands
{
    public class DeleteSpecjalizacjaCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
    }

    public class DeleteSpecjalizacjaCommandHandler : IRequestHandler<DeleteSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetSpecjalizacjaResponse> _cache;
        public DeleteSpecjalizacjaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetSpecjalizacjaResponse> cache)
        {
            _context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(DeleteSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_specjalizacja);

            _context.WeterynarzSpecjalizacjas.RemoveRange(_context.WeterynarzSpecjalizacjas.Where(x => x.IdSpecjalizacja == id));

            var specjalizacja = _context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).First();
            _context.Specjalizacjas.Remove(specjalizacja);

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}