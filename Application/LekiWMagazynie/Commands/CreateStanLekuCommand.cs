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
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateStanLekuCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateStanLekuCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_lek);

            context.LekWMagazynies.Add(new LekWMagazynie
            {
                IdLek = id,
                DataWaznosci = req.request.DataWaznosci.Date,
                Ilosc = req.request.Ilosc
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}