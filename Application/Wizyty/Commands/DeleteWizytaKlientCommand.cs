using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaKlientCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_klient { get; set; }
    }

    public class DeleteWizytaKlientCommandHandler : IRequestHandler<DeleteWizytaKlientCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IWizyta _wizyta;
        private readonly IEmailSender _sender;
        public DeleteWizytaKlientCommandHandler(IKlinikaContext klinikaContext, IHash hash, IWizyta wizyta, IEmailSender emailSender)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
            _sender = emailSender;
        }

        public async Task<int> Handle(DeleteWizytaKlientCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);
            int klientID = _hash.Decode(req.ID_klient);

            //var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).OrderBy(x => x.DataRozpoczecia).ToList();
            var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(id)).Include(x => x.Harmonograms).FirstOrDefault();
            var harmonograms = _context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).OrderBy(x => x.DataRozpoczecia).ToList();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            if (!klientID.Equals(wizyta.IdOsoba))
            {
                throw new UserNotAuthorizedException();
            }

            if (!harmonograms.Any())
            {
                throw new NotFoundException();
            }

            (DateTime rozpoczecie, DateTime zakonczenie) = _wizyta.GetWizytaDates(harmonograms);

            if (!_wizyta.IsWizytaAbleToCancel(rozpoczecie))
            {
                //naliczenie kary lub wysłanie powiadomienia
            }

            foreach (Harmonogram h in harmonograms)
            {
                h.IdWizyta = null;
            }

            wizyta.Status = WizytaStatus.AnulowanaKlient.ToString();
            var result = await _context.SaveChangesAsync(cancellationToken);

            //wysłanie maila z potwierdzeniem anulowania wizyty
            var to = _context.Osobas.Where(x => x.IdOsoba.Equals(klientID)).First().Email;
            await _sender.SendAnulujWizyteEmail(to, harmonograms.ElementAt(0).DataRozpoczecia);

            return result;
        }
    }
}