using Application.Common.Exceptions;
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

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaKlientCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_klient { get; set; }
    }

    public class DeleteWizytaKlientCommandHandler : IRequestHandler<DeleteWizytaKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        private readonly IEmailSender sender;
        public DeleteWizytaKlientCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository, IEmailSender emailSender)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
            sender = emailSender;
        }

        public async Task<int> Handle(DeleteWizytaKlientCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);
            int klientID = hash.Decode(req.ID_klient);

            //var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).OrderBy(x => x.DataRozpoczecia).ToList();
            var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(id)).Include(x => x.Harmonograms).FirstOrDefault();
            var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).OrderBy(x => x.DataRozpoczecia).ToList();

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

            (DateTime rozpoczecie, DateTime zakonczenie) = wizytaRepository.GetWizytaDates(harmonograms);

            if (!wizytaRepository.IsWizytaAbleToCancel(rozpoczecie))
            {
                //naliczenie kary lub wysłanie powiadomienia
            }

            foreach (Harmonogram h in harmonograms)
            {
                h.IdWizyta = null;
            }

            wizyta.Status = WizytaStatus.AnulowanaKlient.ToString();
            var result = await context.SaveChangesAsync(cancellationToken);

            //wysłanie maila z potwierdzeniem anulowania wizyty
            var to = context.Osobas.Where(x => x.IdOsoba.Equals(klientID)).First().Email;
            await sender.SendAnulujWizyteEmail(to, harmonograms.ElementAt(0).DataRozpoczecia);

            return result;
        }
    }
}