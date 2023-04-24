using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Choroby.Commands
{
    public class DeleteDiseaseCommand : IRequest<int>
    {
        public string ID_Choroba { get; set; }
    }

    public class DeleteChorobaCommandHandler : IRequestHandler<DeleteDiseaseCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetDiseaseResponse> _cache;
        public DeleteChorobaCommandHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetDiseaseResponse> cache)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
        }

        public async Task<int> Handle(DeleteDiseaseCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Choroba);

            _context.ChorobaLeks.RemoveRange(_context.ChorobaLeks.Where(x => x.IdChoroba.Equals(id)).ToList());
            _context.WizytaChorobas.RemoveRange(_context.WizytaChorobas.Where(x => x.IdChoroba.Equals(id)).ToList());
            _context.Chorobas.Remove(_context.Chorobas.Where(x => x.IdChoroba.Equals(id)).First());

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}