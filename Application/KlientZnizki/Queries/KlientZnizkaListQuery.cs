using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.KlientZnizki.Queries
{
    public class KlientZnizkaListQuery : IRequest<List<GetKlientZnizkaResponse>>
    {
        public string ID_klient { get; set; }
    }

    public class KlientZnizkaListQueryHandler : IRequestHandler<KlientZnizkaListQuery, List<GetKlientZnizkaResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        public KlientZnizkaListQueryHandler(IKlinikaContext klinikaContext, IHash hash)
        {
            _context = klinikaContext;
            _hash = hash;
        }

        public async Task<List<GetKlientZnizkaResponse>> Handle(KlientZnizkaListQuery req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_klient);

            return (from x in _context.Znizkas
                    join y in _context.KlientZnizkas on x.IdZnizka equals y.IdZnizka into ps
                    from p in ps
                    where p.IdOsoba == id
                    orderby p.DataPrzyznania
                    select new GetKlientZnizkaResponse()
                    {
                        ID_Znizka = _hash.Encode(x.IdZnizka),
                        NazwaZnizki = x.NazwaZnizki,
                        ProcentZnizki = x.ProcentZnizki,
                        CzyWykorzystana = p.CzyWykorzystana
                    }).ToList();
        }
    }
}