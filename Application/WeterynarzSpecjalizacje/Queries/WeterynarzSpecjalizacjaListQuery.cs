using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WeterynarzSpecjalizacje.Queries
{
    public class WeterynarzSpecjalizacjaListQuery : IRequest<List<GetSpecjalizacjaResponse>>
    {
        public string ID_weterynarz { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandle : IRequestHandler<WeterynarzSpecjalizacjaListQuery, List<GetSpecjalizacjaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SpecjalizacjaDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetSpecjalizacjaResponse>> Handle(WeterynarzSpecjalizacjaListQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_weterynarz);

            return (from x in context.WeterynarzSpecjalizacjas
                    join s in context.Specjalizacjas on x.IdSpecjalizacja equals s.IdSpecjalizacja
                    where x.IdOsoba == id
                    orderby s.Nazwa
                    select new GetSpecjalizacjaResponse()
                    {
                        IdSpecjalizacja = hash.Encode(x.IdSpecjalizacja),
                        Nazwa = s.Nazwa,
                        Opis = s.Opis
                    }).ToList();
        }
    }
}