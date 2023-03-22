using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Interfaces;
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
        private readonly IKlinikaContext context;
        private readonly ITokenRepository tokenRepository;
        private readonly IHash hash;
        public RefreshCommandHandler(IKlinikaContext klinikaContext, ITokenRepository repository, IHash _hash)
        {
            context = klinikaContext;
            tokenRepository = repository;
            hash = _hash;
        }

        public async Task<object> Handle(RefreshCommand req, CancellationToken cancellationToken)
        {
            var user = context.Osobas.SingleOrDefault(x => x.RefreshToken.Equals(req.request.RefreshToken));
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
                new Claim("idUser", hash.Encode(user.IdOsoba)),
                new Claim("login", user.NazwaUzytkownika)
            };

            if (user.Rola.Equals("A"))
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else if (user.Rola.Equals("W"))
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "weterynarz"));
            }
            else
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "klient"));
            }

            var token = tokenRepository.GetJWT(userclaim);

            return new 
            { 
                accessToken = token
            };
        }
    }
}