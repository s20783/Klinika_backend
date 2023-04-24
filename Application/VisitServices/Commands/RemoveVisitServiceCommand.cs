using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.WizytaUslugi.Commands
{
    public class RemoveVisitServiceCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string ID_usluga { get; set; }
    }

    public class RemoveWizytaUslugaCommandHandler : IRequestHandler<RemoveVisitServiceCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly IVisit _wizyta;
        public RemoveWizytaUslugaCommandHandler(IKlinikaContext klinikaContext, IHash hash, IVisit wizyta)
        {
            _context = klinikaContext;
            _hash = hash;
            _wizyta = wizyta;
        }

        public async Task<int> Handle(RemoveVisitServiceCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = _hash.Decode(req.ID_wizyta, req.ID_usluga);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _context.WizytaUslugas.Remove(_context.WizytaUslugas.First(x => x.IdWizyta == id1 && x.IdUsluga == id2));
                    await _context.SaveChangesAsync(cancellationToken);

                    var wizyta = _context.Wizyta.Where(x => x.IdWizyta.Equals(id1)).Include(x => x.WizytaUslugas).ThenInclude(y => y.IdUslugaNavigation).First();

                    if (wizyta.Status != VisitStatus.Zaplanowana.ToString() && wizyta.Status != VisitStatus.Zrealizowana.ToString())
                    {
                        throw new Exception("Nie można zmienić usług w anulowanej wizycie");
                    }

                    var uslugas = wizyta.WizytaUslugas.Select(x => x.IdUslugaNavigation).ToList();
                    wizyta.Cena = this._wizyta.GetVisitPrice(uslugas);

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