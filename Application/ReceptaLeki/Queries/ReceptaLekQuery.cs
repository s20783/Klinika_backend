using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ReceptaLeki.Queries
{
    public class ReceptaLekQuery : IRequest<List<GetReceptaLekResponse>>
    {
        public string ID_Recepta { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandle : IRequestHandler<ReceptaLekQuery, List<GetReceptaLekResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SpecjalizacjaDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetReceptaLekResponse>> Handle(ReceptaLekQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_Recepta);

            return (from x in context.ReceptaLeks
                    join y in context.Leks on x.IdLek equals y.IdLek
                    where x.IdWizyta == id
                    orderby y.Nazwa
                    select new GetReceptaLekResponse()
                    {
                        ID_Lek = hash.Encode(x.IdLek),
                        Nazwa = y.Nazwa,
                        JednostkaMiary = y.JednostkaMiary,
                        Producent = y.Producent,
                        Ilosc = x.Ilosc
                    }).ToList();
        }
    }
}