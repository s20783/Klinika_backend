using Application.DTO.Requests;
using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Commands
{
    public class CreateDiseaseCommand : IRequest<int>
    {
        public DiseaseRequest request { get; set; }
    }

    public class CreateChorobaCommandHandler : IRequestHandler<CreateDiseaseCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetDiseaseResponse> _cache;
        public CreateChorobaCommandHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetDiseaseResponse> cache)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
        }

        public async Task<int> Handle(CreateDiseaseCommand req, CancellationToken cancellationToken)
        {
            if(_context.Chorobas.Where(x => x.Nazwa.Equals(req.request.Nazwa)).Any())
            {
                throw new Exception("already exists");
            }

            _context.Chorobas.Add(new Choroba
            {
                Nazwa = req.request.Nazwa,
                NazwaLacinska = req.request.NazwaLacinska,
                Opis = req.request.Opis
            });

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}