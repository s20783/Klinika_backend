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
    public class DeleteVisitAdminCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
    }

    public class DeleteWizytaAdminCommandHandler : IRequestHandler<DeleteVisitAdminCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IEmailSender _sender;
        public DeleteWizytaAdminCommandHandler(IKlinikaContext klinikaContext, IHash hash, IEmailSender emailSender)
        {
            _context = klinikaContext;
            _hash = hash;
            _sender = emailSender;
        }

        public async Task<int> Handle(DeleteVisitAdminCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_wizyta);

            //var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(id)).OrderBy(x => x.DataRozpoczecia).ToList();
            var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(id)).FirstOrDefault();
            var harmonograms = _context.Harmonograms.Where(x => x.IdWizyta == id).OrderBy(x => x.DataRozpoczecia).ToList();

            if (!((VisitStatus)Enum.Parse(typeof(VisitStatus), wizyta.Status, true)).Equals(VisitStatus.Zaplanowana))
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

            wizyta.Status = VisitStatus.AnulowanaKlinika.ToString();
            await _context.SaveChangesAsync(cancellationToken);

            //wysłanie maila z potwierdzeniem anulowania wizyty do klienta
            var to = _context.Osobas.Where(x => x.IdOsoba.Equals(wizyta.IdOsoba)).First().Email;
            await _sender.SendCancelledVisitEmail(to, harmonograms.First().DataRozpoczecia);

            return 0;
        }
    }
}