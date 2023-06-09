﻿using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GodzinaPracy.Commands
{
    public class CreateWorkingHoursCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public WorkingHoursRequest Request { get; set; }
    }

    public class CreateGodzinyPracyCommandHandle : IRequestHandler<CreateWorkingHoursCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(CreateWorkingHoursCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            if (_context.GodzinyPracies.Where(x => x.DzienTygodnia == req.Request.DzienTygodnia && x.IdOsoba == id).Any())
            {
                throw new Exception("Ten weterynarz ma już ustawione godziny pracy tego dnia");
            }

            _context.GodzinyPracies.Add(new GodzinyPracy
            {
                IdOsoba = id,
                DzienTygodnia = req.Request.DzienTygodnia,
                GodzinaRozpoczecia = req.Request.GodzinaRozpoczecia,
                GodzinaZakonczenia = req.Request.GodzinaZakonczenia
            });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}