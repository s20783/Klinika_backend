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
using Domain.Enums;

namespace Application.Konto.Commands
{
    public class LoginCommand : IRequest<LoginTokens>
    {
        public LoginRequest request { get; set; }
    }

    public class LoginCommandHandle : IRequestHandler<LoginCommand, LoginTokens>
    {
        private readonly IKlinikaContext _context;
        private readonly IToken _tokenRepository;
        private readonly IPassword _passwordRepository;
        private readonly IConfiguration _configuration;
        private readonly IHash _hash;
        private readonly ILogin _loginRepository;
        public LoginCommandHandle(IKlinikaContext klinikaContext, IToken token, IPassword password, IConfiguration config, IHash hash, ILogin login)
        {
            _context = klinikaContext;
            _tokenRepository = token;
            _passwordRepository = password;
            _configuration = config;
            _hash = hash;
            _loginRepository = login;
        }

        public async Task<LoginTokens> Handle(LoginCommand req, CancellationToken cancellationToken)
        {
            var user = _context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).FirstOrDefault();

            if (!_loginRepository.CheckCredentails(user, _passwordRepository, req.request.Haslo, int.Parse(_configuration["PasswordIterations"])))
            {
                await _context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect");
            }

            List<Claim> userclaim = new List<Claim>
            {
                new Claim("idUser", _hash.Encode(user.IdOsoba)),
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

            var token = _tokenRepository.GetJWT(userclaim);

            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync(cancellationToken);

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