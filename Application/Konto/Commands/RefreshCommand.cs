using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class RefreshCommand : IRequest<object>
    {
        public RefreshTokenRequest request { get; set; }
    }

    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, object>
    {
        private readonly IKlinikaContext _context;
        private readonly IToken _tokenRepository;
        private readonly IHash _hash;
        public RefreshCommandHandler(IKlinikaContext klinikaContext, IToken repository, IHash hash)
        {
            _context = klinikaContext;
            _tokenRepository = repository;
            _hash = hash;
        }

        public async Task<object> Handle(RefreshCommand req, CancellationToken cancellationToken)
        {
            var user = _context.Osobas.SingleOrDefault(x => x.RefreshToken.Equals(req.request.RefreshToken));
            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono Refresh Token");
            }

            if (user.RefreshTokenExp < DateTime.Now)
            {
                throw new UserNotAuthorizedException("Refresh Token wygasł");
            }

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", _hash.Encode(user.IdOsoba)),
                new Claim("login", user.NazwaUzytkownika)
            };

            if (user.IdRola == ((int)RolaEnum.Admin))
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else if (user.IdRola == ((int)RolaEnum.Weterynarz))
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "Weterynarz"));
            }
            else
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "Klient"));
            }

            var token = _tokenRepository.GetJWT(userclaim);

            return new 
            { 
                accessToken = token
            };
        }
    }
}