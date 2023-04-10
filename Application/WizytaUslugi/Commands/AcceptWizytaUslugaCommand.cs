using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaUslugi.Commands
{
    public class AcceptWizytaUslugaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
    }

    public class AcceptWizytaUslugaCommandHandler : IRequestHandler<AcceptWizytaUslugaCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public AcceptWizytaUslugaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(AcceptWizytaUslugaCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            var wizyta = _context.Wizyta.First(x => x.IdWizyta == id);

            if (wizyta.CzyZaakceptowanaCena)
            {
                throw new Exception();
            }

            var klientZnizka = _context.KlientZnizkas
                //.Include(x => x.IdZnizkaNavigation)
                .FirstOrDefault(x => x.IdOsoba.Equals(wizyta.IdOsoba) && x.CzyWykorzystana == false);

            if (klientZnizka != null)
            {
                var znizka = _context.Znizkas.First(x => x.IdZnizka == klientZnizka.IdZnizka);
                wizyta.IdZnizka = znizka.IdZnizka;
                wizyta.CenaZnizka = wizyta.Cena * (1 - (znizka.ProcentZnizki / 100));
                klientZnizka.CzyWykorzystana = true;
            }

            wizyta.CzyZaakceptowanaCena = true;
            wizyta.Status = WizytaStatus.Zrealizowana.ToString();

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}