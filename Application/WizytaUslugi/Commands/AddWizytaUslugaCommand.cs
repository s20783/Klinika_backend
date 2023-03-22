using Application.DTO.Responses;
using Application.Interfaces;
using Application.WizytaUslugi.Queries;
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
    public class AddWizytaUslugaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_usluga { get; set; }
    }

    public class AddWizytaUslugaCommandHandler : IRequestHandler<AddWizytaUslugaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public AddWizytaUslugaCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<int> Handle(AddWizytaUslugaCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_wizyta, req.ID_usluga);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    context.WizytaUslugas.Add(new WizytaUsluga
                    {
                        IdWizyta = id1,
                        IdUsluga = id2
                    });

                    await context.SaveChangesAsync(cancellationToken);

                    var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(id1)).Include(x => x.WizytaUslugas).ThenInclude(y => y.IdUslugaNavigation).First();
                    
                    if(wizyta.Status != WizytaStatus.Zaplanowana.ToString() && wizyta.Status != WizytaStatus.Zrealizowana.ToString())
                    {
                        throw new Exception("Nie można dodać usługi do anulowanej wizyty");
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