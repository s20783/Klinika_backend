using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Commands
{
    public class UpdateWorkingHoursCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public WorkingHoursRequest Request { get; set; }
    }

    public class UpdateGodzinyPracyCommandHandle : IRequestHandler<UpdateWorkingHoursCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public UpdateGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(UpdateWorkingHoursCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);
            var day = await _context.GodzinyPracies
                .Where(x => x.IdOsoba == id && x.DzienTygodnia == req.Request.DzienTygodnia)
                .FirstOrDefaultAsync(cancellationToken);

            if (day == null)
            {
                throw new Exception("Ten pracownik nie ma ustawionych godzin pracy tego dnia.");
            }

            day.GodzinaRozpoczecia = req.Request.GodzinaRozpoczecia;
            day.GodzinaZakonczenia = req.Request.GodzinaZakonczenia;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}