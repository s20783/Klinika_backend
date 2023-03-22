using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChorobaLeki.Queries
{
    public class ChorobaLekListQuery : IRequest<List<GetChorobaResponse>>
    {
        public string ID_lek { get; set; }
    }

    public class SpecjalizacjaDetailsQueryHandle : IRequestHandler<ChorobaLekListQuery, List<GetChorobaResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public SpecjalizacjaDetailsQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetChorobaResponse>> Handle(ChorobaLekListQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_lek);

            return (from x in context.ChorobaLeks
                    join s in context.Chorobas on x.IdChoroba equals s.IdChoroba
                    where x.IdLek == id
                    orderby s.Nazwa
                    select new GetChorobaResponse()
                    {
                        ID_Choroba = hash.Encode(x.IdChoroba),
                        Nazwa = s.Nazwa,
                        NazwaLacinska = s.NazwaLacinska,
                        Opis = s.Opis
                    }).ToList();
        }
    }
}