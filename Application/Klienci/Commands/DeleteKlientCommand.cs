using Application.Common.Exceptions;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.IO;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace Application.Klienci.Commands
{
    public class DeleteKlientCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class DeleteKlientCommandHandle : IRequestHandler<DeleteKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetKlientListResponse> cache;
        public DeleteKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetKlientListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<int> Handle(DeleteKlientCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var osoba = context.Osobas.Include(x => x.IdRolaNavigation).First(x => x.IdOsoba == id);
            var klient = context.Klients.First(x => x.IdOsoba == id);

            //dodać enum
            if (!string.IsNullOrEmpty(osoba.IdRolaNavigation.Nazwa))
            {
                throw new Exception("");
            }
            
            using (StreamWriter fileStream = new StreamWriter(new FileStream(@"Klienci.log", FileMode.Append)))
            {
                string outputString = osoba.Imie + " " + osoba.Nazwisko.ElementAt(0).ToString() + 
                    " (" + klient.DataZalozeniaKonta.ToShortDateString() + " - " + DateTime.Now.ToShortDateString() + ")" + "\n";
                //string total = "koszt wizyt: " + context.Wizyta.Where(x => x.IdOsoba == id).Sum(x => x.Cena).ToString() + "\n";
                string stats = "liczba wizyt: " + context.Wizyta.Where(x => x.IdOsoba == id).Count() + "\n" + "\n";

                await fileStream.WriteAsync(outputString + stats);
            }

            osoba.Nazwisko = osoba.Nazwisko.ElementAt(0).ToString();
            osoba.Haslo = "";
            osoba.Salt = "";
            osoba.RefreshToken = new Guid().ToString();
            osoba.Email = "";
            osoba.NumerTelefonu = "";

            //return 0;

            cache.Remove();
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}