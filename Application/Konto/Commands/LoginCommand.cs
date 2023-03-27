using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Domain;
using Domain.Enums;

namespace Application.Konto.Commands
{
    public class LoginCommand : IRequest<LoginTokens>
    {
        public LoginRequest request { get; set; }
    }

    public class LoginCommandHandle : IRequestHandler<LoginCommand, LoginTokens>
    {
        private readonly IKlinikaContext context;
        private readonly ITokenRepository tokenRepository;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        private readonly ILoginRepository loginRepository;
        public LoginCommandHandle(IKlinikaContext klinikaContext, ITokenRepository token, IPasswordRepository password, IConfiguration config, IHash _hash, ILoginRepository login)
        {
            context = klinikaContext;
            tokenRepository = token;
            passwordRepository = password;
            configuration = config;
            hash = _hash;
            loginRepository = login;
        }

        public async Task<LoginTokens> Handle(LoginCommand req, CancellationToken cancellationToken)
        {
            var user = context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).FirstOrDefault();

            if (!loginRepository.CheckCredentails(user, passwordRepository, req.request.Haslo, int.Parse(configuration["PasswordIterations"])))
            {
                await context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect");
            }

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", hash.Encode(user.IdOsoba)),
                new Claim("login", user.NazwaUzytkownika)
            };

            string userRola = "";


            if (user.IdRola == ((int)RolaEnum.Admin))
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "Admin"));
                userRola = "admin";
            }
            else if (user.IdRola == ((int)RolaEnum.Weterynarz))
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "Weterynarz"));
                userRola = "weterynarz";
            }
            else
            {
                userclaim.Add(new Claim(ClaimTypes.Role, "Klient"));
                userRola = "user";
            }

            var token = tokenRepository.GetJWT(userclaim);

            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            await context.SaveChangesAsync(cancellationToken);

            return new LoginTokens()
            {
                Token = token,
                RefreshToken = refreshToken,
                Imie = user.Imie,
                Rola = userRola
            };
        }
    }
}