using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class UpdatePatientCommand : IRequest<int>
    { 
        public PatientRequest request { get; set; }
        public string ID_pacjent { get; set; }
    }

    public class UpdatePacjentCommandHandle : IRequestHandler<UpdatePatientCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetPatientListResponse> _cache;
        public UpdatePacjentCommandHandle(IKlinikaContext klinikaContext, IHash ihash, ICache<GetPatientListResponse> cache)
        {
            _context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(UpdatePatientCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_pacjent);

            var pacjent = _context.Pacjents.Where(x => x.IdPacjent == id).FirstOrDefault();
            if (pacjent is null)
            {
                throw new Exception();
            }

            pacjent.IdOsoba = _hash.Decode(req.request.IdOsoba);
            pacjent.Nazwa = req.request.Nazwa;
            pacjent.Gatunek = req.request.Gatunek;
            pacjent.Rasa = req.request.Rasa;
            pacjent.Masc = req.request.Masc;
            pacjent.Plec = req.request.Plec;
            pacjent.DataUrodzenia = req.request.DataUrodzenia.Date;
            pacjent.Waga = req.request.Waga;
            pacjent.Agresywne = req.request.Agresywne;
            pacjent.Ubezplodnienie = req.request.Ubezplodnienie;

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}