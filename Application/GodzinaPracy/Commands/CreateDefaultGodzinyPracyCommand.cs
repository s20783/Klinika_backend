using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;
using Domain;
using Domain.Models;

namespace Application.GodzinaPracy.Commands
{
    public class CreateDefaultGodzinyPracyCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class CreateDefaultGodzinyPracyCommandHandle : IRequestHandler<CreateDefaultGodzinyPracyCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateDefaultGodzinyPracyCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateDefaultGodzinyPracyCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            for (int i = 1; i <= GlobalValues.DNI_PRACY; i++)
            {
                context.GodzinyPracies.Add(new GodzinyPracy
                {
                    IdOsoba = id,
                    DzienTygodnia = i,
                    GodzinaRozpoczecia = GlobalValues.GODZINA_ROZPOCZECIA_PRACY,
                    GodzinaZakonczenia = GlobalValues.GODZINA_ZAKONCZENIA_PRACY
                });
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}