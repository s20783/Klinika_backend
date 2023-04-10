using Application.DTO.Request;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Commands
{
    public class CreateStanLekuCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public StanLekuRequest request { get; set; }
    }

    public class CreateStanLekuCommandHandler : IRequestHandler<CreateStanLekuCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateStanLekuCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(CreateStanLekuCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_lek);

            _context.LekWMagazynies.Add(new LekWMagazynie
            {
                IdLek = id,
                DataWaznosci = req.request.DataWaznosci.Date,
                Ilosc = req.request.Ilosc
            });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}