using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class CreateLekCommand : IRequest<string>
    {
        public LekRequest request { get; set; }
    }

    public class CreateLekCommandHandler : IRequestHandler<CreateLekCommand, string>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateLekCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<string> Handle(CreateLekCommand req, CancellationToken cancellationToken)
        {
            var result = _context.Leks.Add(new Lek
            {
                Nazwa = req.request.Nazwa,
                JednostkaMiary = req.request.JednostkaMiary,
                Producent = req.request.Producent
            });

            await _context.SaveChangesAsync(cancellationToken);
            return result != null ? _hash.Encode(result.Entity.IdLek) : string.Empty;
        }
    }
}