using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.WizytaUslugi.Commands
{
    public class AcceptWizytaUslugaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
    }

    public class AcceptWizytaUslugaCommandHandler : IRequestHandler<AcceptWizytaUslugaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public AcceptWizytaUslugaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(AcceptWizytaUslugaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            var wizyta = context.Wizyta.First(x => x.IdWizyta == id);

            if (wizyta.CzyZaakceptowanaCena)
            {
                throw new Exception();
            }

            var klientZnizka = context.KlientZnizkas
                //.Include(x => x.IdZnizkaNavigation)
                .FirstOrDefault(x => x.IdOsoba.Equals(wizyta.IdOsoba) && x.CzyWykorzystana == false);

            if (klientZnizka != null)
            {
                var znizka = context.Znizkas.First(x => x.IdZnizka == klientZnizka.IdZnizka);
                wizyta.IdZnizka = znizka.IdZnizka;
                wizyta.CenaZnizka = wizyta.Cena * (1 - (znizka.ProcentZnizki / 100));
                klientZnizka.CzyWykorzystana = true;
            }

            wizyta.CzyZaakceptowanaCena = true;
            wizyta.Status = WizytaStatus.Zrealizowana.ToString();

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}