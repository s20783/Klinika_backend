using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class DeleteMedicamentCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
    }

    public class DeleteLekCommandHandler : IRequestHandler<DeleteMedicamentCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeleteMedicamentCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_lek);

            _context.LekWMagazynies.RemoveRange(_context.LekWMagazynies.Where(x => x.IdLek.Equals(id)).ToList());
            _context.ChorobaLeks.RemoveRange(_context.ChorobaLeks.Where(x => x.IdLek.Equals(id)).ToList());
            _context.WizytaLeks.RemoveRange(_context.WizytaLeks.Where(x => x.IdLek.Equals(id)).ToList());
            _context.ReceptaLeks.RemoveRange(_context.ReceptaLeks.Where(x => x.IdLek.Equals(id)).ToList());

            if(_context.Szczepionkas.Where(x => x.IdLek.Equals(id)).Any())
            {
                _context.Szczepionkas.Remove(_context.Szczepionkas.Where(x => x.IdLek.Equals(id)).First());
            }
            _context.Leks.Remove(_context.Leks.Where(x => x.IdLek.Equals(id)).First());

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}