using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;

namespace Application.GodzinaPracy.Commands
{
    public class CreateGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public List<GodzinyPracyRequest> requestList { get; set; }
    }

    public class CreateGodzinyPracyCommandHandle : IRequestHandler<CreateGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(CreateGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);
            var i = 0;

            foreach (GodzinyPracyRequest request in req.requestList)
            {

                i = request.DzienTygodnia;
                if (_context.GodzinyPracies.Where(x => x.DzienTygodnia == i && x.IdOsoba == id).Any())
                {
                    throw new Exception("Ten weterynarz ma już ustawione godziny pracy tego dnia");
                }

                _context.GodzinyPracies.Add(new GodzinyPracy
                {
                    IdOsoba = id,
                    DzienTygodnia = i,
                    GodzinaRozpoczecia = request.GodzinaRozpoczecia,
                    GodzinaZakonczenia = request.GodzinaZakonczenia
                });
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}