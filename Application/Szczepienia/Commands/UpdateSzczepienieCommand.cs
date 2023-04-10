using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System.Linq;
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
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public UpdateSzczepienieCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            this._hash = hash;
        }

        public async Task<int> Handle(UpdateSzczepienieCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_szczepienie);

            var szczepienie = _context.Szczepienies.Where(x => x.IdSzczepienie.Equals(id)).First();
            szczepienie.IdLek = _hash.Decode(req.request.IdLek);
            szczepienie.IdPacjent = _hash.Decode(req.request.IdPacjent);
            szczepienie.Data = req.request.Data;
            szczepienie.Dawka = req.request.Dawka;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}