using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Recepty.Commands
{
    public class UpdatePrescriptionCommand : IRequest<int>
    {
        public string ID_recepta { get; set; }
        public string Zalecenia { get; set; }
    }

    public class UpdateReceptaCommandHandler : IRequestHandler<UpdatePrescriptionCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public UpdateReceptaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(UpdatePrescriptionCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_recepta);

            var recepta = _context.Recepta.Where(x => x.IdWizyta.Equals(id)).First();
            recepta.Zalecenia = req.Zalecenia;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}