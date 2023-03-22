using Application.Common.Exceptions;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class DeleteWeterynarzCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class DeleteWeterynarzCommandHandle : IRequestHandler<DeleteWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetWeterynarzListResponse> cache;
        private readonly IHarmonogramRepository harmonogram;
        public DeleteWeterynarzCommandHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetWeterynarzListResponse> _cache, IHarmonogramRepository _harmonogram)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
            harmonogram = _harmonogram;
        }

        public async Task<int> Handle(DeleteWeterynarzCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var weterynarz = context.Osobas.Where(x => x.IdOsoba == id).First();
            weterynarz.Nazwisko = weterynarz.Nazwisko.ElementAt(0).ToString();
            weterynarz.Haslo = "";
            weterynarz.Salt = "";
            weterynarz.RefreshToken = "";
            weterynarz.Email = "";
            weterynarz.NumerTelefonu = "";

            var x = context.GodzinyPracies.Where(x => x.IdOsoba == id).ToList();
            if (x.Any())
            {
                context.GodzinyPracies.RemoveRange(x);
            }

            var harmonograms = context.Harmonograms.Where(x => x.WeterynarzIdOsoba == id && x.DataRozpoczecia > DateTime.Now).ToList();
            if(harmonograms.Any()) 
            {
                await harmonogram.DeleteHarmonograms(harmonograms, context);
            }

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}