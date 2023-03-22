using Application.DTO.Requests;
using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Uslugi.Commands
{
    public class CreateUslugaCommand : IRequest<int>
    {
        public UslugaRequest request { get; set; }
    }

    public class CreateUslugaCommandHandler : IRequestHandler<CreateUslugaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetUslugaResponse> cache;
        public CreateUslugaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetUslugaResponse> _cache)
        {
            context = klinikaContext;
            hash = ihash;
            cache = _cache;
        }

        public async Task<int> Handle(CreateUslugaCommand req, CancellationToken cancellationToken)
        {
            context.Uslugas.Add(new Usluga
            {
                NazwaUslugi = req.request.NazwaUslugi,
                Opis = req.request.Opis,
                Cena = req.request.Cena,
                Dolegliwosc = req.request.Dolegliwosc,
                Narkoza = req.request.Narkoza
            });

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}