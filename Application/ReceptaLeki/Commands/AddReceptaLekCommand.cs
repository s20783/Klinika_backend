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
    public class AddReceptaLekCommand : IRequest<int>
    {
        public string ID_Recepta { get; set; }
        public string ID_Lek { get; set; }
        public int Ilosc { get; set; }
    }

    public class CreateReceptaLekCommandHandler : IRequestHandler<AddReceptaLekCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateReceptaLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(AddReceptaLekCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Recepta);
            int id2 = _hash.Decode(req.ID_Lek);

            _context.ReceptaLeks.Add(new Domain.Models.ReceptaLek
            {
                IdWizyta = id,
                IdLek = id2,
                Ilosc = req.Ilosc,
            });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}