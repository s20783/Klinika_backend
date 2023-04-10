using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepionki.Commands
{
    public class UpdateSzczepionkaCommand : IRequest<int>
    {
        public string ID_szczepionka { get; set; }
        public SzczepionkaRequest request { get; set; }
    }

    public class UpdateSzczepionkaCommandHandler : IRequestHandler<UpdateSzczepionkaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public UpdateSzczepionkaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(UpdateSzczepionkaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_szczepionka);

            var lek = _context.Leks.Where(x => x.IdLek.Equals(id)).First();
            var szczepionka = _context.Szczepionkas.Where(x => x.IdLek.Equals(id)).First();

            lek.Nazwa = req.request.Nazwa;
            lek.Producent = req.request.Producent;
            szczepionka.Zastosowanie = req.request.Zastosowanie;
            szczepionka.OkresWaznosci = req.request.OkresWaznosci != null ? TimeSpan.FromDays((double)req.request.OkresWaznosci).Ticks : null;
            szczepionka.CzyObowiazkowa = req.request.CzyObowiazkowa;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}