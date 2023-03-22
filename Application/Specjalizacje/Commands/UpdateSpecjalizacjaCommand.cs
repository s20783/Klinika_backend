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
    public class UpdateSpecjalizacjaCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
        public SpecjalizacjaRequest request { get; set; }
    }

    public class UpdateSpecjalizacjaCommandHandle : IRequestHandler<UpdateSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetSpecjalizacjaResponse> cache;
        public UpdateSpecjalizacjaCommandHandle(IKlinikaContext klinikaContext, IHash ihash, ICache<GetSpecjalizacjaResponse> _cache)
        {
            context = klinikaContext;
            hash = ihash;
            cache = _cache;
        }

        public async Task<int> Handle(UpdateSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_specjalizacja);

            var specjalizacja = context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).FirstOrDefault();
            specjalizacja.Nazwa = req.request.Nazwa;
            specjalizacja.Opis = req.request.Opis;

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}