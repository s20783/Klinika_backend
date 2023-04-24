using Application.Interfaces;
using Domain.Models;
using Domain.Enums;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Common.Exceptions;
using Domain;
using System.Transactions;

namespace Application.Wizyty.Commands
{
    public class CreateVisitClientCommand : IRequest<int>
    {
        public string ID_klient { get; set; }
        public string ID_pacjent { get; set; }
        public string ID_Harmonogram { get; set; }
        public string Notatka { get; set; }
    }

    public class UmowWizyteKlientCommandHandler : IRequestHandler<CreateVisitClientCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IVisit _wizyta;
        private readonly IEmailSender _sender;
        public UmowWizyteKlientCommandHandler(IKlinikaContext klinikaContext, IHash hash, IVisit wizyta, IEmailSender emailSender)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
            _sender = emailSender;
        }

        public async Task<int> Handle(CreateVisitClientCommand req, CancellationToken cancellationToken)
        {
            int id1 = _hash.Decode(req.ID_klient);
            int id_harmonogram = _hash.Decode(req.ID_Harmonogram);

            if (!_wizyta.IsVisitAbleToCreate(_context.Wizyta.Where(x => x.IdOsoba == id1).ToList()))
            {
                throw new ConstraintException("Nie można umówić wizyty, osiągnięto limit umówionych wizyt", GlobalValues.MAX_UMOWIONYCH_WIZYT);
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = _context.Wizyta.Add(new Wizytum
                    {
                        IdOsoba = id1,
                        IdPacjent = req.ID_pacjent != "0" ? _hash.Decode(req.ID_pacjent) : null,
                        Opis = "",
                        NotatkaKlient = req.Notatka,
                        Status = VisitStatus.Zaplanowana.ToString(),
                        Cena = 0,
                        CzyOplacona = false,
                        CzyZaakceptowanaCena = false
                    });

                    await _context.SaveChangesAsync(cancellationToken);

                    var harmonogram = _context.Harmonograms.Where(x => x.IdHarmonogram == id_harmonogram).First();
                    harmonogram.IdWizyta = result != null ? result.Entity.IdWizyta : 0;

                    await _context.SaveChangesAsync(cancellationToken);
                    
                    //wysłanie maila z potwierdzeniem umówienia wizyty
                    var to = _context.Osobas.Where(x => x.IdOsoba.Equals(id1)).First().Email;
                    var weterynarz = _context.Osobas.Where(x => x.IdOsoba.Equals(harmonogram.WeterynarzIdOsoba)).Select(x => x.Imie + " " + x.Nazwisko).First();
                    await _sender.SendCreatedVisitEmail(to, harmonogram.DataRozpoczecia, weterynarz);
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message);
                }

                transaction.Dispose();
                return 0;
            }
        }
    }
}