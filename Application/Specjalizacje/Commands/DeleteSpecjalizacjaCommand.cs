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

    public class DeleteSpecjalizacjaCommandHandle : IRequestHandler<DeleteSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetSpecjalizacjaResponse> cache;
        public DeleteSpecjalizacjaCommandHandle(IKlinikaContext klinikaContext, IHash ihash, ICache<GetSpecjalizacjaResponse> _cache)
        {
            context = klinikaContext;
            hash = ihash;
            cache = _cache;
        }

        public async Task<int> Handle(DeleteSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_specjalizacja);

            context.WeterynarzSpecjalizacjas.RemoveRange(context.WeterynarzSpecjalizacjas.Where(x => x.IdSpecjalizacja == id));

            var specjalizacja = context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).First();
            context.Specjalizacjas.Remove(specjalizacja);

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}