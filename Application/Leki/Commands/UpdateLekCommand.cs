using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class UpdateLekCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public LekRequest request { get; set; }
    }

    public class UpdateLekCommandHandler : IRequestHandler<UpdateLekCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public UpdateLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(UpdateLekCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_lek);

            var lek = _context.Leks.Where(x => x.IdLek.Equals(id)).First();
            lek.Nazwa = req.request.Nazwa;
            lek.JednostkaMiary = req.request.JednostkaMiary;
            lek.Producent = req.request.Producent;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}