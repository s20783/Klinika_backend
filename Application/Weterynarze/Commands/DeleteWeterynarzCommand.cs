﻿using Application.Common.Exceptions;
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

    public class DeleteWeterynarzCommandHandler : IRequestHandler<DeleteWeterynarzCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetWeterynarzListResponse> _cache;
        private readonly IHarmonogram _harmonogram;
        public DeleteWeterynarzCommandHandler(IKlinikaContext klinikaContext, IHash hash, ICache<GetWeterynarzListResponse> cache, IHarmonogram harmonogram)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
            _harmonogram = harmonogram;
        }

        public async Task<int> Handle(DeleteWeterynarzCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            var weterynarz = _context.Osobas.Where(x => x.IdOsoba == id).First();
            weterynarz.Nazwisko = weterynarz.Nazwisko.ElementAt(0).ToString();
            weterynarz.Haslo = "";
            weterynarz.Salt = "";
            weterynarz.RefreshToken = "";
            weterynarz.Email = "";
            weterynarz.NumerTelefonu = "";

            var x = _context.GodzinyPracies.Where(x => x.IdOsoba == id).ToList();
            if (x.Any())
            {
                _context.GodzinyPracies.RemoveRange(x);
            }

            var harmonograms = _context.Harmonograms.Where(x => x.WeterynarzIdOsoba == id && x.DataRozpoczecia > DateTime.Now).ToList();
            if(harmonograms.Any()) 
            {
                await _harmonogram.DeleteHarmonograms(harmonograms, _context);
            }

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}