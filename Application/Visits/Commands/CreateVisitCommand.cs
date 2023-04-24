using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Wizyty.Commands
{
    public class CreateVisitCommand : IRequest<string>
    {
        public string? ID_pacjent { get; set; }
        public string ID_harmonogram { get; set; }
        public string ID_klient { get; set; }
    }

    public class CreateWizytaCommandHandler : IRequestHandler<CreateVisitCommand, string>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IVisit _wizyta;
        public CreateWizytaCommandHandler(IKlinikaContext klinikaContext, IHash hash, IVisit wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
        }

        public async Task<string> Handle(CreateVisitCommand req, CancellationToken cancellationToken)
        {
            var harmonogramID = _hash.Decode(req.ID_harmonogram);
            var klientID = _hash.Decode(req.ID_klient);
            var id = 0;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = _context.Wizyta.Add(new Wizytum
                    {
                        IdOsoba = klientID,
                        IdPacjent = req.ID_pacjent != "0" ? _hash.Decode(req.ID_pacjent) : null,
                        Opis = "",
                        Status = VisitStatus.Zaplanowana.ToString(),
                        Cena = 0,
                        CzyOplacona = false,
                        CzyZaakceptowanaCena = false
                    });

                    await _context.SaveChangesAsync(cancellationToken);

                    id = result != null ? result.Entity.IdWizyta : 0;
                    var harmonogram = _context.Harmonograms.First(x => x.IdHarmonogram == harmonogramID);
                    harmonogram.IdWizyta = id;

                    await _context.SaveChangesAsync(cancellationToken);
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message);
                }

                transaction.Dispose();
                return _hash.Encode(id);
            }
        }
    }
}