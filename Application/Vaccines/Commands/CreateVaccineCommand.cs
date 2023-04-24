using Application.DTO.Requests;
using Application.Interfaces;
using Domain;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Szczepionki.Commands
{
    public class CreateVaccineCommand : IRequest<int>
    {
        public VaccineRequest request { get; set; }
    }

    public class CreateSzczepionkaCommandHandler : IRequestHandler<CreateVaccineCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateSzczepionkaCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(CreateVaccineCommand req, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var lek = _context.Leks.Add(new Lek
                    {
                        Nazwa = req.request.Nazwa,
                        JednostkaMiary = GlobalValues.SZCZEPIONKA_JEDNOSTKA,
                        Producent = req.request.Producent
                    });

                    await _context.SaveChangesAsync(cancellationToken);

                    var id = lek != null ? lek.Entity.IdLek : 0;

                    _context.Szczepionkas.Add(new Szczepionka
                    {
                        IdLek = id,
                        Zastosowanie = req.request.Zastosowanie,
                        CzyObowiazkowa = req.request.CzyObowiazkowa,
                        OkresWaznosci = req.request.OkresWaznosci != null ? TimeSpan.FromDays((double)req.request.OkresWaznosci).Ticks : null
                    });


                    await _context.SaveChangesAsync(cancellationToken);
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw new Exception();
                }

                transaction.Dispose();
                return 0;
            }
        }
    }
}