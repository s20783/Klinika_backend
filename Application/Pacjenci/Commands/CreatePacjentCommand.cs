﻿using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class CreatePacjentCommand : IRequest<int>
    {
        public PacjentCreateRequest request { get; set; }
    }

    public class CreatePacjentCommandHandle : IRequestHandler<CreatePacjentCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetPacjentListResponse> _cache;
        public CreatePacjentCommandHandle(IKlinikaContext klinikaContext, IHash hash, ICache<GetPacjentListResponse> cache)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
        }

        public async Task<int> Handle(CreatePacjentCommand req, CancellationToken cancellationToken)
        {
            _context.Pacjents.Add(new Pacjent
            {
                IdOsoba = _hash.Decode(req.request.IdOsoba),
                Nazwa = req.request.Nazwa,
                Gatunek = req.request.Gatunek,
                Rasa = req.request.Rasa,
                Waga = req.request.Waga,
                Masc = req.request.Masc,
                DataUrodzenia = req.request.DataUrodzenia.Date,
                Plec = req.request.Plec,
                Agresywne = req.request.Agresywne,
                Ubezplodnienie = req.request.Ubezplodnienie
            });

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}