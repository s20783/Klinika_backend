using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Commands
{
    public class UpdateSpecjalizacjaCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
        public SpecjalizacjaRequest request { get; set; }
    }

    public class UpdateSpecjalizacjaCommandHandler : IRequestHandler<UpdateSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetSpecjalizacjaResponse> _cache;
        public UpdateSpecjalizacjaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetSpecjalizacjaResponse> cache)
        {
            _context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(UpdateSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_specjalizacja);

            var specjalizacja = _context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).FirstOrDefault();
            specjalizacja.Nazwa = req.request.Nazwa;
            specjalizacja.Opis = req.request.Opis;

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}