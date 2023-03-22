using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WizytaLeki.Queries
{
    public class WizytaLekListQuery : IRequest<List<GetLekListResponse>>
    {
        public string ID_wizyta { get; set; }
    }

    public class WizytaLekListQueryHandler : IRequestHandler<WizytaLekListQuery, List<GetLekListResponse>>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public WizytaLekListQueryHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<List<GetLekListResponse>> Handle(WizytaLekListQuery req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            return (from x in context.Leks
                    join y in context.WizytaLeks on x.IdLek equals y.IdLek
                    orderby x.Nazwa
                    where y.IdWizyta == id
                    select new GetLekListResponse()
                    {
                        IdLek = hash.Encode(x.IdLek),
                        Nazwa = x.Nazwa,
                        JednostkaMiary = x.JednostkaMiary,
                        Producent = x.Producent,
                        Ilosc = y.Ilosc
                    }).AsParallel().WithCancellation(cancellationToken).ToList();
        }
    }
}