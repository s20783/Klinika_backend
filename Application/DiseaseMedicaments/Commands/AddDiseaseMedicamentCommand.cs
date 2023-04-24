using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChorobaLeki.Commands
{
    public class AddDiseaseMedicamentCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public string ID_choroba { get; set; }
    }

    public class AddChorobaLekCommandHandler : IRequestHandler<AddDiseaseMedicamentCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public AddChorobaLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(AddDiseaseMedicamentCommand req, CancellationToken cancellationToken)
        {
            int lekID = _hash.Decode(req.ID_lek);
            int chorobaID = _hash.Decode(req.ID_choroba);

            _context.ChorobaLeks.Add(new ChorobaLek
            {
                IdChoroba = chorobaID,
                IdLek = lekID
            });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}