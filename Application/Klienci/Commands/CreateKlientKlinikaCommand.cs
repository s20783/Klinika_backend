using Application.DTO.Responses;
using Application.DTO;
using Application.Interfaces;
using Application.Weterynarze.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Requests;

namespace Application.Klienci.Commands
{
    public class CreateKlientKlinikaCommand : IRequest<int>
    {
        public KlientCreateKlinikaRequest request { get; set; }
    }

    public class CreateWeterynarzCommandHandler : IRequestHandler<CreateKlientKlinikaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        private readonly IEmailSender emailSender;
        private readonly ICache<GetKlientListResponse> cache;
        public CreateWeterynarzCommandHandler(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config, IHash _hash, IEmailSender sender, ICache<GetKlientListResponse> _cache)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
            hash = _hash;
            emailSender = sender;
            cache = _cache;
        }

        public async Task<int> Handle(CreateKlientKlinikaCommand req, CancellationToken cancellationToken)
        {
            var generatedLogin = "Klient" + (context.Osobas.Max(x => x.IdOsoba) + 1);

            if (context.Osobas.Where(x => x.NazwaUzytkownika.Equals(generatedLogin)).Any())
            {
                throw new Exception("Not unique");
            }

            var generatedPassword = passwordRepository.GetRandomPassword(8);
            byte[] salt = passwordRepository.GenerateSalt();
            var hashedPassword = passwordRepository.HashPassword(salt, generatedPassword, int.Parse(configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            var query = "exec DodajKlienta @imie, @nazwisko, @numerTel, @email, @login, @haslo, @salt";

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@imie", req.request.Imie);
            command.Parameters.AddWithValue("@nazwisko", req.request.Nazwisko);
            command.Parameters.AddWithValue("@numerTel", req.request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", req.request.Email);
            command.Parameters.AddWithValue("@login", generatedLogin);
            command.Parameters.AddWithValue("@haslo", hashedPassword);
            command.Parameters.AddWithValue("@salt", saltBase64);

            command.ExecuteScalar();
            await connection.CloseAsync();
            await emailSender.SendCreateAccountEmail(req.request.Email);
            cache.Remove();
            return 0;
        }
    }
}