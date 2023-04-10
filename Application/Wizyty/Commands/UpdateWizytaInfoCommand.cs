using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class UpdateWizytaInfoCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_weterynarz { get; set; }
        public WizytaInfoUpdateRequest request { get; set; }
    }

    public class UpdateWizytaInfoCommandHandler : IRequestHandler<UpdateWizytaInfoCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IWizyta _wizyta;
        public UpdateWizytaInfoCommandHandler(IKlinikaContext klinikaContext, IHash hash, IWizyta wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
        }

        public async Task<int> Handle(UpdateWizytaInfoCommand req, CancellationToken cancellationToken)
        {
            (int wizytaID, int weterynarzID) = _hash.Decode(req.ID_wizyta, req.ID_weterynarz);

            var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(wizytaID)).Include(x => x.Harmonograms).FirstOrDefault();
            var harmonogram = _context.Harmonograms.Where(x => x.IdWizyta == wizytaID).ToList();

            foreach (var h in harmonogram)
            {
                if (!h.WeterynarzIdOsoba.Equals(weterynarzID))
                {
                    throw new UserNotAuthorizedException();
                }
            }

            /*List<Usluga> uslugas = new List<Usluga>();
            for(int i = 0; i < req.request.Uslugi.Length; i++)
            {
                context.WizytaUslugas.Add(new WizytaUsluga
                {
                    IdUsluga = hash.Decode(req.request.Uslugi[i]),
                    IdWizyta = wizytaID
                });

                uslugas.Add(context.Uslugas.Where(x => x.IdUsluga.Equals(hash.Decode(req.request.Uslugi[i]))).First());
            }*/

            wizyta.Opis = req.request.Opis;
            wizyta.IdPacjent = req.request.ID_Pacjent != "0" ? _hash.Decode(req.request.ID_Pacjent) : null;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}