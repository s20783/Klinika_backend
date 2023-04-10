using Application.DTO;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class CreateWeterynarzCommand : IRequest<object>
    {
        public WeterynarzCreateRequest request { get; set; }
    }

    public class CreateWeterynarzCommandHandler : IRequestHandler<CreateWeterynarzCommand, object>
    {
        private readonly IKlinikaContext _context;
        private readonly IPassword _password;
        private readonly IConfiguration _configuration;
        private readonly IHash _hash;
        private readonly IEmailSender _emailSender;
        private readonly ICache<GetWeterynarzListResponse> _cache;
        public CreateWeterynarzCommandHandler(IKlinikaContext klinikaContext, IPassword password, IConfiguration config, IHash hash, IEmailSender sender, ICache<GetWeterynarzListResponse> cache)
        {
            _context = klinikaContext;
            _password = password;
            _configuration = config;
            _hash = hash;
            _emailSender = sender;
            _cache = cache;
        }

        public async Task<object> Handle(CreateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            var generatedLogin = "PetMed1";

            if (_context.Weterynarzs.Any())
            {
                generatedLogin = "PetMed" + (_context.Weterynarzs.Max(x => x.IdOsoba) + 1);
            }
            
            if (_context.Osobas.Where(x => x.NazwaUzytkownika.Equals(generatedLogin)).Any())
            {
                throw new Exception("Not unique");
            }

            var generatedPassword = _password.GetRandomPassword(8);
            byte[] salt = _password.GenerateSalt();
            var hashedPassword = _password.HashPassword(salt, generatedPassword, int.Parse(_configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            var query = "exec DodajWeterynarza @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @pensja, @dataZatrudnienia, @salt";

            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("KlinikaDatabase"));
            connection.Open();
            //SqlTransaction trans = connection.BeginTransaction();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@imie", req.request.Imie);
            command.Parameters.AddWithValue("@nazwisko", req.request.Nazwisko);
            command.Parameters.AddWithValue("@dataUr", req.request.DataUrodzenia.Date);
            command.Parameters.AddWithValue("@numerTel", req.request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", req.request.Email);
            command.Parameters.AddWithValue("@login", generatedLogin);
            command.Parameters.AddWithValue("@haslo", hashedPassword);
            command.Parameters.AddWithValue("@pensja", req.request.Pensja);
            command.Parameters.AddWithValue("@dataZatrudnienia", req.request.DataZatrudnienia.Date);
            command.Parameters.AddWithValue("@salt", saltBase64);

            int resultID = Convert.ToInt32(command.ExecuteScalar());
            await connection.CloseAsync();
            await _emailSender.SendHasloEmail(req.request.Email, generatedPassword);
            _cache.Remove();
            return new 
            { 
                ID = _hash.Encode(resultID) 
            };
        }
    }
}