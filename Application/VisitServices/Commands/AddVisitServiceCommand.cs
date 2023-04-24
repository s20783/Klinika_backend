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
    public class AddVisitServiceCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_usluga { get; set; }
    }

    public class AddWizytaUslugaCommandHandler : IRequestHandler<AddVisitServiceCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IVisit _wizyta;
        public AddWizytaUslugaCommandHandler(IKlinikaContext klinikaContext, IHash hash, IVisit wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
        }

        public async Task<int> Handle(AddVisitServiceCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = _hash.Decode(req.ID_wizyta, req.ID_usluga);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _context.WizytaUslugas.Add(new WizytaUsluga
                    {
                        IdWizyta = id1,
                        IdUsluga = id2
                    });

                    await _context.SaveChangesAsync(cancellationToken);

                    var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(id1)).Include(x => x.WizytaUslugas).ThenInclude(y => y.IdUslugaNavigation).First();
                    
                    if(wizyta.Status != VisitStatus.Zaplanowana.ToString() && wizyta.Status != VisitStatus.Zrealizowana.ToString())
                    {
                        throw new Exception("Nie można dodać usługi do anulowanej wizyty");
                    }
                    
                    var uslugas = wizyta.WizytaUslugas.Select(x => x.IdUslugaNavigation).ToList();
                    wizyta.Cena = _wizyta.GetVisitPrice(uslugas);

                    await _context.SaveChangesAsync(cancellationToken);
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