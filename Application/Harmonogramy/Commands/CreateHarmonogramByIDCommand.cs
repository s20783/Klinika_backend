using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class CreateHarmonogramByIDCommand : IRequest<int>
    {
        public string ID_weterynarz { get; set; }
        public DateTime Data { get; set; }
    }

    public class CreateHarmonogramByIDCommandHandler : IRequestHandler<CreateHarmonogramByIDCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogramService;
        public CreateHarmonogramByIDCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogramService = harmonogramRepository;
        }

        public async Task<int> Handle(CreateHarmonogramByIDCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_weterynarz);

            if (context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.Data) && x.WeterynarzIdOsoba.Equals(id)).Any())
            {
                throw new Exception("Harmonogram na ten dzień już istnieje");
            }

            harmonogramService.CreateWeterynarzHarmonograms(context, req.Data, id);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}