using Application.DTO.Request;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.LekiWMagazynie.Commands
{
    public class CreateMedicamentWarehouseCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public MedicamentWarehouseRequest request { get; set; }
    }

    public class CreateStanLekuCommandHandler : IRequestHandler<CreateMedicamentWarehouseCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public CreateStanLekuCommandHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<int> Handle(CreateMedicamentWarehouseCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_lek);

            _context.LekWMagazynies.Add(new LekWMagazynie
            {
                IdLek = id,
                DataWaznosci = req.request.DataWaznosci.Date,
                Ilosc = req.request.Ilosc
            });

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}