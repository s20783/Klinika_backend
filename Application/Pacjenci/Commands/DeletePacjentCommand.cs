﻿using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pacjenci.Commands
{
    public class DeletePacjentCommand : IRequest<int>
    {
        public string ID_Pacjent { get; set; }
    }

    public class DeletePacjentCommandHandle : IRequestHandler<DeletePacjentCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash _hash;
        private readonly ICache<GetPacjentListResponse> _cache;
        public DeletePacjentCommandHandle(IKlinikaContext klinikaContext, IHash ihash, ICache<GetPacjentListResponse> cache)
        {
            context = klinikaContext;
            _hash = ihash;
            _cache = cache;
        }

        public async Task<int> Handle(DeletePacjentCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_Pacjent);

            context.Szczepienies.RemoveRange(context.Szczepienies.Where(x => x.IdPacjent == id).ToList());
            context.Pacjents.Remove(context.Pacjents.Where(x => x.IdPacjent == id).First());

            int result = await context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}