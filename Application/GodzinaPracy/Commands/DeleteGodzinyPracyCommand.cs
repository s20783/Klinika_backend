using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Commands
{
    public class DeleteGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public int dzien { get; set; }
    }

    public class DeleteGodzinyPracyCommandHandle : IRequestHandler<DeleteGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);
            var godzinyPracy = context.GodzinyPracies.Where(x => x.DzienTygodnia == req.dzien && x.IdOsoba == id).FirstOrDefault();
            if (godzinyPracy == null)
            {
                throw new Exception("Ten pracownik nie ma ustawionych godzin pracy.");
            }

            context.GodzinyPracies.Remove(godzinyPracy);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}