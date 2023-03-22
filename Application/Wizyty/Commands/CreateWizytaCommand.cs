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
    public class CreateWizytaCommand : IRequest<string>
    {
        public string? ID_pacjent { get; set; }
        public string ID_harmonogram { get; set; }
        public string ID_klient { get; set; }
    }

    public class CreateWizytaCommandHandler : IRequestHandler<CreateWizytaCommand, string>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public CreateWizytaCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<string> Handle(CreateWizytaCommand req, CancellationToken cancellationToken)
        {
            var harmonogramID = hash.Decode(req.ID_harmonogram);
            var klientID = hash.Decode(req.ID_klient);
            var id = 0;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = context.Wizyta.Add(new Wizytum
                    {
                        IdOsoba = klientID,
                        IdPacjent = req.ID_pacjent != "0" ? hash.Decode(req.ID_pacjent) : null,
                        Opis = "",
                        Status = WizytaStatus.Zaplanowana.ToString(),
                        Cena = 0,
                        CzyOplacona = false,
                        CzyZaakceptowanaCena = false
                    });

                    await context.SaveChangesAsync(cancellationToken);

                    id = result != null ? result.Entity.IdWizyta : 0;
                    var harmonogram = context.Harmonograms.First(x => x.IdHarmonogram == harmonogramID);
                    harmonogram.IdWizyta = id;

                    await context.SaveChangesAsync(cancellationToken);
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw e;
                }

                transaction.Dispose();
                return hash.Encode(id);
            }
        }
    }
}