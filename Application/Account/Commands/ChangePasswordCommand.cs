using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class ChangePasswordCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public ChangePasswordRequest request { get; set; }
    }

    public class ChangePasswordCommandHandle : IRequestHandler<ChangePasswordCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IPassword _passwordRepository;
        private readonly IConfiguration _configuration;
        private readonly IHash _hash;
        private readonly ILogin _loginRepository;
        public ChangePasswordCommandHandle(IKlinikaContext klinikaContext, IPassword password, IConfiguration config, IHash hash, ILogin login)
        {
            _context = klinikaContext;
            _passwordRepository = password;
            _configuration = config;
            _hash = hash;
            _loginRepository = login;
        }

        public async Task<int> Handle(ChangePasswordCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            var user = _context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if(!_loginRepository.CheckCredentails(user, _passwordRepository, req.request.CurrentHaslo, int.Parse(_configuration["PasswordIterations"]))){
                await _context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect password");
            }

            string hashedPassword = _passwordRepository.HashPassword(Convert.FromBase64String(user.Salt), req.request.NewHaslo, int.Parse(_configuration["PasswordIterations"]));
            user.Haslo = hashedPassword;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}