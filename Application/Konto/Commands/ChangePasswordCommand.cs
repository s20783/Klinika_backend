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
        public KontoChangePasswordRequest request { get; set; }
    }

    public class ChangePasswordCommandHandle : IRequestHandler<ChangePasswordCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        private readonly ILoginRepository loginRepository;
        public ChangePasswordCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config, IHash _hash, ILoginRepository login)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
            hash = _hash;
            loginRepository = login;
        }

        public async Task<int> Handle(ChangePasswordCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if(!loginRepository.CheckCredentails(user, passwordRepository, req.request.CurrentHaslo, int.Parse(configuration["PasswordIterations"]))){
                await context.SaveChangesAsync(cancellationToken);
                throw new UserNotAuthorizedException("Incorrect password");
            }

            string hashedPassword = passwordRepository.HashPassword(Convert.FromBase64String(user.Salt), req.request.NewHaslo, int.Parse(configuration["PasswordIterations"]));
            user.Haslo = hashedPassword;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}