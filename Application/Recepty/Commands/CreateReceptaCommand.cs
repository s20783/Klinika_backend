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
using System.Transactions;

namespace Application.Recepty.Commands
{
    public class CreateReceptaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string Zalecenia { get; set; }
    }

    public class CreateReceptaCommandHandler : IRequestHandler<CreateReceptaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateReceptaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(CreateReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            _context.Recepta.Add(new Receptum
            {
                IdWizyta = id,
                Zalecenia = req.Zalecenia
            });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}