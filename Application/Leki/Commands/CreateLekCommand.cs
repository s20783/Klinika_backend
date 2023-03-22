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
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<string> Handle(CreateLekCommand req, CancellationToken cancellationToken)
        {
            var result = context.Leks.Add(new Lek
            {
                Nazwa = req.request.Nazwa,
                JednostkaMiary = req.request.JednostkaMiary,
                Producent = req.request.Producent
            });

            await context.SaveChangesAsync(cancellationToken);
            return result != null ? hash.Encode(result.Entity.IdLek) : "";
        }
    }
}