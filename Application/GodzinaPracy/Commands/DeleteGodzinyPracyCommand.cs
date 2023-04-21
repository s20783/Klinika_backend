using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Commands
{
    public class DeleteGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public int Day { get; set; }
    }

    public class DeleteGodzinyPracyCommandHandle : IRequestHandler<DeleteGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public DeleteGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(DeleteGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);
            var godzinyPracy = _context.GodzinyPracies.Where(x => x.DzienTygodnia == req.Day && x.IdOsoba == id).FirstOrDefault();
            if (godzinyPracy == null)
            {
                throw new Exception("Ten pracownik nie ma ustawionych godzin pracy.");
            }

            _context.GodzinyPracies.Remove(godzinyPracy);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}