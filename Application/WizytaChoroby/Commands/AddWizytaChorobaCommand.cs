using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaChoroby.Commands
{
    public class AddWizytaChorobaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_choroba { get; set; }
    }

    public class AddWizytaChorobaCommandHandler : IRequestHandler<AddWizytaChorobaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public AddWizytaChorobaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(AddWizytaChorobaCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = _hash.Decode(req.ID_wizyta, req.ID_choroba);

            _context.WizytaChorobas.Add(
                new WizytaChoroba
                {
                    IdWizyta = id1,
                    IdChoroba = id2
                });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}