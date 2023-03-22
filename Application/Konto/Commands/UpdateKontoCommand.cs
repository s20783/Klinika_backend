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
    public class UpdateKontoCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public KontoUpdateRequest request { get; set; }
    }

    public class UpdateKontoCommandHandle : IRequestHandler<UpdateKontoCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        private readonly ILoginRepository loginRepository;
        private readonly ICache<GetKlientListResponse> cache;
        public UpdateKontoCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config, IHash _hash, ILoginRepository login, ICache<GetKlientListResponse> _cache)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
            hash = _hash;
            loginRepository = login;
            cache = _cache;
        }

        public async Task<int> Handle(UpdateKontoCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (!loginRepository.CheckCredentails(user, passwordRepository, req.request.Haslo, int.Parse(configuration["PasswordIterations"])))
            {
                await context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect password");
            }

            if (!user.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika))
            {
                if (context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).Any())
                {
                    throw new Exception("Not unique");
                }
            }

            user.NumerTelefonu = req.request.NumerTelefonu;
            user.Email = req.request.Email;
            user.NazwaUzytkownika = req.request.NazwaUzytkownika;

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}