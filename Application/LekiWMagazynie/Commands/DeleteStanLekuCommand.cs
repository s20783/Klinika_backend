using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Commands
{
    public class DeleteStanLekuCommand : IRequest<int>
    {
        public string ID_stan_leku { get; set; }
    }

    public class DeleteStanLekuCommandHandler : IRequestHandler<DeleteStanLekuCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteStanLekuCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteStanLekuCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_stan_leku);

            context.LekWMagazynies.Remove(context.LekWMagazynies.Where(x => x.IdStanLeku == id).First());
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}