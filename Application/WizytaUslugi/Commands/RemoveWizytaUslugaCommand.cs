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
    public class RemoveWizytaUslugaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_usluga { get; set; }
    }

    public class RemoveWizytaUslugaCommandHandler : IRequestHandler<RemoveWizytaUslugaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public RemoveWizytaUslugaCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<int> Handle(RemoveWizytaUslugaCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_wizyta, req.ID_usluga);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    context.WizytaUslugas.Remove(context.WizytaUslugas.First(x => x.IdWizyta == id1 && x.IdUsluga == id2));
                    await context.SaveChangesAsync(cancellationToken);

                    var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(id1)).Include(x => x.WizytaUslugas).ThenInclude(y => y.IdUslugaNavigation).First();

                    if (wizyta.Status != WizytaStatus.Zaplanowana.ToString() && wizyta.Status != WizytaStatus.Zrealizowana.ToString())
                    {
                        throw new Exception("Nie można zmienić usług w anulowanej wizycie");
                    }

                    var uslugas = wizyta.WizytaUslugas.Select(x => x.IdUslugaNavigation).ToList();
                    wizyta.Cena = wizytaRepository.GetWizytaCena(uslugas);

                    await context.SaveChangesAsync(cancellationToken);
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