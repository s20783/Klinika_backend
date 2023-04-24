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
    public class CreateServiceCommand : IRequest<int>
    {
        public ServiceRequest request { get; set; }
    }

    public class CreateUslugaCommandHandler : IRequestHandler<CreateServiceCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly ICache<GetServiceResponse> _cache;
        public CreateUslugaCommandHandler(IKlinikaContext klinikaContext, ICache<GetServiceResponse> cache)
        {
            _context = klinikaContext;
            _cache = cache;
        }

        public async Task<int> Handle(CreateServiceCommand req, CancellationToken cancellationToken)
        {
            _context.Uslugas.Add(new Usluga
            {
                NazwaUslugi = req.request.NazwaUslugi,
                Opis = req.request.Opis,
                Cena = req.request.Cena,
                Dolegliwosc = req.request.Dolegliwosc,
                Narkoza = req.request.Narkoza
            });

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}