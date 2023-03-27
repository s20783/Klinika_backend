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

    public class CreateWeterynarzCommandHandle : IRequestHandler<CreateWeterynarzCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly IPasswordRepository passwordRepository;
        private readonly IConfiguration configuration;
        private readonly IHash hash;
        private readonly IEmailSender emailSender;
        private readonly ICache<GetWeterynarzListResponse> cache;
        public CreateWeterynarzCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IConfiguration config, IHash _hash, IEmailSender sender, ICache<GetWeterynarzListResponse> _cache)
        {
            context = klinikaContext;
            passwordRepository = password;
            configuration = config;
            hash = _hash;
            emailSender = sender;
            cache = _cache;
        }

        public async Task<object> Handle(CreateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            var generatedLogin = "PetMed1";

            if (context.Weterynarzs.Any())
            {
                generatedLogin = "PetMed" + (context.Weterynarzs.Max(x => x.IdOsoba) + 1);
            }
            
            if (context.Osobas.Where(x => x.NazwaUzytkownika.Equals(generatedLogin)).Any())
            {
                throw new Exception("Not unique");
            }

            var generatedPassword = passwordRepository.GetRandomPassword(8);
            byte[] salt = passwordRepository.GenerateSalt();
            var hashedPassword = passwordRepository.HashPassword(salt, generatedPassword, int.Parse(configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            var query = "exec DodajWeterynarza @imie, @nazwisko, @dataUr, @numerTel, @email, @login, @haslo, @pensja, @dataZatrudnienia, @salt";

            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("KlinikaDatabase"));
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
            await emailSender.SendHasloEmail(req.request.Email, generatedPassword);
            cache.Remove();
            return new 
            { 
                ID = hash.Encode(resultID) 
            };
        }
    }
}