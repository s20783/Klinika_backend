using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepienia.Commands
{
    public class UpdateVaccinationCommand : IRequest<int>
    {
        public string ID_szczepienie { get; set; }
        public VaccinationRequest request { get; set; }
    }

    public class UpdateSzczepienieCommandHandler : IRequestHandler<UpdateVaccinationCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public UpdateSzczepienieCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            this._hash = hash;
        }

        public async Task<int> Handle(UpdateVaccinationCommand req, CancellationToken cancellationToken)
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