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
    public class UpdateStanLekuCommand : IRequest<int>
    {
        public string ID_stan_leku { get; set; }
        public StanLekuRequest request { get; set; }
    }

    public class UpdateStanLekuCommandHandler : IRequestHandler<UpdateStanLekuCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateStanLekuCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateStanLekuCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_stan_leku);

            var stanLeku = context.LekWMagazynies.Where(x => x.IdStanLeku == id).FirstOrDefault();
            stanLeku.Ilosc = req.request.Ilosc;
            stanLeku.DataWaznosci = req.request.DataWaznosci.Date;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}