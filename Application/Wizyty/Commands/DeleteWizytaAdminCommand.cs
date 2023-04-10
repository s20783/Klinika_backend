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
    public class DeleteWizytaAdminCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
    }

    public class DeleteWizytaAdminCommandHandler : IRequestHandler<DeleteWizytaAdminCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IWizyta _wizyta;
        private readonly IEmailSender _sender;
        public DeleteWizytaAdminCommandHandler(IKlinikaContext klinikaContext, IHash hash, IWizyta wizyta, IEmailSender emailSender)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
            _sender = emailSender;
        }

        public async Task<int> Handle(DeleteWizytaAdminCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            //var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).OrderBy(x => x.DataRozpoczecia).ToList();
            var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();
            var harmonograms = _context.Harmonograms.Where(x => x.IdWizyta == id).OrderBy(x => x.DataRozpoczecia).ToList();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            if (!harmonograms.Any())
            {
                throw new NotFoundException();
            }

            foreach (Harmonogram h in harmonograms)
            {
                h.IdWizyta = null;
            }

            wizyta.Status = WizytaStatus.AnulowanaKlinika.ToString();
            await _context.SaveChangesAsync(cancellationToken);

            //wysłanie maila z potwierdzeniem anulowania wizyty do klienta
            var to = _context.Osobas.Where(x => x.IdOsoba.Equals(wizyta.IdOsoba)).First().Email;
            await _sender.SendAnulujWizyteEmail(to, harmonograms.First().DataRozpoczecia);

            return 0;
        }
    }
}