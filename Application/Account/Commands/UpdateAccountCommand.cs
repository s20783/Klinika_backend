using Application.Common.Exceptions;
using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class UpdateAccountCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public AccountUpdateRequest request { get; set; }
    }

    public class UpdateKontoCommandHandle : IRequestHandler<UpdateAccountCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IPassword _passwordRepository;
        private readonly IConfiguration _configuration;
        private readonly IHash _hash;
        private readonly ILogin _loginRepository;
        private readonly ICache<GetClientListResponse> _cache;
        public UpdateKontoCommandHandle(IKlinikaContext klinikaContext, IPassword password, IConfiguration config, IHash hash, ILogin login, ICache<GetClientListResponse> cache)
        {
            _context = klinikaContext;
            _passwordRepository = password;
            _configuration = config;
            _hash = hash;
            _loginRepository = login;
            _cache = cache;
        }

        public async Task<int> Handle(UpdateAccountCommand req, CancellationToken cancellationToken)
        {
            int id = _hash.Decode(req.ID_osoba);

            var user = _context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (!_loginRepository.CheckCredentails(user, _passwordRepository, req.request.Haslo, int.Parse(_configuration["PasswordIterations"])))
            {
                await _context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect password");
            }

            if (!user.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika))
            {
                if (_context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).Any())
                {
                    throw new Exception("Not unique");
                }
            }

            user.NumerTelefonu = req.request.NumerTelefonu;
            user.Email = req.request.Email;
            user.NazwaUzytkownika = req.request.NazwaUzytkownika;

            int result = await _context.SaveChangesAsync(cancellationToken);
            _cache.Remove();

            return result;
        }
    }
}