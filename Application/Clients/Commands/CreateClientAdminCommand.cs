using Application.DTO.Requests;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Commands
{
    public class CreateClientAdminCommand : IRequest<int>
    {
        public ClientAdminRequest request { get; set; }
    }

    public class CreateClientAdminCommandHandler : IRequestHandler<CreateClientAdminCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IPassword _passwordRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ICache<GetClientListResponse> _cache;
        public CreateClientAdminCommandHandler(IKlinikaContext klinikaContext, IPassword password, IConfiguration config, IEmailSender sender, ICache<GetClientListResponse> cache)
        {
            _context = klinikaContext;
            _passwordRepository = password;
            _configuration = config;
            _emailSender = sender;
            _cache = cache;
        }

        public async Task<int> Handle(CreateClientAdminCommand req, CancellationToken cancellationToken)
        {
            var generatedLogin = "Klient" + (_context.Osobas.Max(x => x.IdOsoba) + 1);

            if (_context.Osobas.Where(x => x.NazwaUzytkownika.Equals(generatedLogin)).Any())
            {
                throw new Exception("Not unique");
            }

            var generatedPassword = _passwordRepository.GetRandomPassword(8);
            byte[] salt = _passwordRepository.GenerateSalt();
            var hashedPassword = _passwordRepository.HashPassword(salt, generatedPassword, int.Parse(_configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            var query = "exec DodajKlienta @imie, @nazwisko, @numerTel, @email, @login, @haslo, @salt";

            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("KlinikaDatabase"));
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
            await _emailSender.SendCreateAccountEmail(req.request.Email);
            _cache.Remove();
            return 0;
        }
    }
}