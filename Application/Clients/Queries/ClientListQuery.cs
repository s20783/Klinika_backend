using Application.DTO.Responses;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Queries
{
    public class ClientListQuery : IRequest<PaginatedResponse<GetClientListResponse>>
    {
        public string? SearchWord { get; set; }
        public int Page { get; set; } = 1;
    }

    public class KlientListQueryHandle : IRequestHandler<ClientListQuery, PaginatedResponse<GetClientListResponse>>
    {
        private readonly IKlinikaContext _context;
        private readonly IHash _hash;
        private readonly ICache<GetClientListResponse> _cache;
        private readonly IMapper _mapper;
        public KlientListQueryHandle(IKlinikaContext klinikaContext, IHash hash, ICache<GetClientListResponse> cache, IMapper mapper)
        {
            _context = klinikaContext;
            _hash = hash;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<GetClientListResponse>> Handle(ClientListQuery req, CancellationToken cancellationToken)
        {
            List<GetClientListResponse> data = _cache.GetFromCache();

            if (data is null)
            {
                data = _mapper.Map<List<GetClientListResponse>>(await _context.Klients
                    .Include(x => x.IdOsobaNavigation)
                    .Where(x => x.IdOsobaNavigation.IdRola == ((int)RoleEnum.Klient))
                    .ToListAsync(cancellationToken)
                    );

                _cache.AddToCache(data);
            }

            var results = data
                .Where(
                x => req.SearchWord == null ||
                x.Nazwisko.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Imie.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.NumerTelefonu.ToLower().Contains(req.SearchWord.ToLower()) ||
                x.Email.ToLower().Contains(req.SearchWord.ToLower())
                )
                .OrderBy(x => x.Nazwisko);

            return new PaginatedResponse<GetClientListResponse>
            {
                Items = results
                    .Skip((req.Page - 1) * GlobalValues.PAGE_SIZE)
                    .Take(GlobalValues.PAGE_SIZE)
                    .ToList(),
                PageCount = (int)Math.Ceiling(results.Count() / (double)GlobalValues.PAGE_SIZE),
                PageIndex = req.Page
            };
        }
    }
}