using Application.DTO.Request;
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

namespace Application.Specjalizacje.Commands
{
    public class CreateSpecjalizacjaCommand : IRequest<int>
    {
        public SpecjalizacjaRequest request { get; set; }
    }

    public class CreateSpecjalizacjaCommandHandler : IRequestHandler<CreateSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetSpecjalizacjaResponse> _cache;
        public CreateSpecjalizacjaCommandHandler(IKlinikaContext klinikaContext, IHash ihash, ICache<GetSpecjalizacjaResponse> cache)
        {
            _context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(CreateSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            _context.Specjalizacjas.Add(new Specjalizacja
            {
                Nazwa = req.request.Nazwa,
                Opis = req.request.Opis
            });

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}