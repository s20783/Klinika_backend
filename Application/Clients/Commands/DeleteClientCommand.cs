using Application.Common.Exceptions;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.IO;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Klienci.Commands
{
    public class DeleteClientCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class DeleteKlientCommandHandle : IRequestHandler<DeleteClientCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetClientListResponse> _cache;
        public DeleteKlientCommandHandle(IKlinikaContext klinikaContext, IHash hash, ICache<GetClientListResponse> cache)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
        }

        public async Task<int> Handle(DeleteClientCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            var osoba = _context.Osobas.Include(x => x.IdRolaNavigation).First(x => x.IdOsoba == id);
            var klient = _context.Klients.First(x => x.IdOsoba == id);
            
            using (StreamWriter fileStream = new StreamWriter(new FileStream(@"Klienci.log", FileMode.Append)))
            {
                string outputString = osoba.Imie + " " + osoba.Nazwisko.ElementAt(0).ToString() + 
                    " (" + klient.DataZalozeniaKonta.ToShortDateString() + " - " + DateTime.Now.ToShortDateString() + ")" + "\n";
                string total = "koszt wizyt: " + _context.Wizyta.Where(x => x.IdOsoba == id).Sum(x => x.Cena).ToString() + "\n";
                string stats = "liczba wizyt: " + _context.Wizyta.Where(x => x.IdOsoba == id).Count() + "\n" + "\n";

                await fileStream.WriteAsync(outputString + stats + total);
            }

            osoba.Nazwisko = osoba.Nazwisko.ElementAt(0).ToString();
            osoba.Haslo = "";
            osoba.Salt = "";
            osoba.RefreshToken = new Guid().ToString();
            osoba.Email = "";
            osoba.NumerTelefonu = "";

            _cache.Remove();
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}