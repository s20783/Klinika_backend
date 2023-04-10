using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Wizyty.Commands
{
    public class UpdateWizytaDateCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }   //stare
        public string ID_klient { get; set; }
        public string ID_pacjent { get; set; }
        public string ID_harmonogram { get; set; }  //nowe
        public string Notatka { get; set; }
    }

    public class PrzelozWizyteCommandHandler : IRequestHandler<UpdateWizytaDateCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IWizyta _wizytaRepository;
        public PrzelozWizyteCommandHandler(IKlinikaContext klinikaContext, IHash hash, IWizyta wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizytaRepository = wizyta;
        }

        public async Task<int> Handle(UpdateWizytaDateCommand req, CancellationToken cancellationToken)
        {
            int wizytaID = _hash.Decode(req.ID_wizyta);
            int harmonogramID = _hash.Decode(req.ID_harmonogram);

            var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(wizytaID)).First();
            var oldHarmonograms = _context.Harmonograms.Where(x => x.IdWizyta.Equals(wizytaID)).ToList();

            if (!wizyta.Status.Equals(WizytaStatus.Zaplanowana.ToString()))
            {
                throw new Exception();
            }

            /*if (!klientID.Equals(wizyta.IdOsoba))
            {
                throw new UserNotAuthorizedException();
            }*/

            if (!oldHarmonograms.Any())
            {
                throw new NotFoundException();
            }

            (DateTime rozpoczecie, DateTime zakonczenie) = _wizytaRepository.GetWizytaDates(oldHarmonograms);
            if (!_wizytaRepository.IsWizytaAbleToCancel(rozpoczecie))
            {
                //naliczenie kary lub wysłanie powiadomienia
            }

            //wyliczenie długości przekładanej wizyty
            //int wizytaLength = oldHarmonograms.Count();

            //ID weterynarza z nowej wizyty
            //int weterynarzID = context.Harmonograms.Where(x => x.IdHarmonogram.Equals(harmonogramID)).First().WeterynarzIdOsoba;
            //var newDataRozpoczecia = context.Harmonograms.Where(x => x.IdHarmonogram.Equals(harmonogramID)).First().DataRozpoczecia;

            //usuwanie poprzednich zarezerwowanych terminów z harmonogramu
            foreach (Harmonogram h in oldHarmonograms)
            {
                h.IdWizyta = null;
            }

            //nowe harmonogramy wyciągnięte na podstawie nowego ID harmonogramu
            /*var newHarmonograms = context.Harmonograms
                .Where(x => x.WeterynarzIdOsoba.Equals(weterynarzID) && x.DataRozpoczecia >= newDataRozpoczecia)
                .OrderBy(x => x.DataRozpoczecia)
                .Take(wizytaLength)
                .ToList();*/

            //sprawdzenie czy są dostępne terminy do danego weterynarz w harmonogramie
            /*if (!wizytaRepository.IsWizytaAbleToReschedule(newHarmonograms, newDataRozpoczecia))
            {
                throw new NotFoundException();
            }*/

            /*for (int i = 0; i <= newHarmonograms.Count; i++)
            {
                newHarmonograms.ElementAt(i).IdWizyta = wizytaID;
            }*/

            var harmonogram = _context.Harmonograms.First(x => x.IdHarmonogram == harmonogramID);
            harmonogram.IdWizyta = wizytaID;

            wizyta.IdPacjent = req.ID_pacjent != "0" ? _hash.Decode(req.ID_pacjent) : null;
            wizyta.NotatkaKlient = req.Notatka;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}