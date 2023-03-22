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
    public class UpdateReceptaCommand : IRequest<int>
    {
        public string ID_recepta { get; set; }
        public string Zalecenia { get; set; }
        //public List<ReceptaLekRequest2> Leki { get; set; }
    }

    public class UpdateReceptaCommandHandler : IRequestHandler<UpdateReceptaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateReceptaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_recepta);

            //context.ReceptaLeks.RemoveRange(context.ReceptaLeks.Where(x => x.IdWizyta == id).ToList());

            var recepta = context.Recepta.Where(x => x.IdWizyta.Equals(id)).First();
            recepta.Zalecenia = req.Zalecenia;

            /*foreach (var i in req.Leki)
            {
                context.ReceptaLeks.Add(new ReceptaLek
                {
                    IdWizyta = id,
                    IdLek = hash.Decode(i.ID_Lek),
                    Ilosc = i.Ilosc,
                });
            }*/

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}