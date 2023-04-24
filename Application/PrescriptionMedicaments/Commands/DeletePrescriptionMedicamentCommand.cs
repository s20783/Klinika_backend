using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ReceptaLeki.Commands
{
    public class DeletePrescriptionMedicamentCommand : IRequest<int>
    {
        public string ID_Recepta { get; set; }
        public string ID_Lek { get; set; }
    }

    public class DeleteReceptaLekCommandHandler : IRequestHandler<DeletePrescriptionMedicamentCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteReceptaLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeletePrescriptionMedicamentCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Recepta);
            int id2 = _hash.Decode(req.ID_Lek);

            var receptaLek = _context.ReceptaLeks.Where(x => x.IdWizyta.Equals(id) && x.IdLek.Equals(id2)).First();
            _context.ReceptaLeks.Remove(receptaLek);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}