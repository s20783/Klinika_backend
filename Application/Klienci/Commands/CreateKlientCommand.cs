﻿using Application.DTO;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Commands
{
    public class CreateKlientCommand : IRequest<int>
    {
        public KlientCreateRequest request { get; set; }
    }

    public class CreateKlientCommandHandler : IRequestHandler<CreateKlientCommand, int>
    {
        private readonly IKlinikaContext _context;
        private readonly IPassword _passwordRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ICache<GetKlientListResponse> _cache;
        public CreateKlientCommandHandler(IKlinikaContext klinikaContext, IPassword password, IConfiguration config, IEmailSender sender, ICache<GetKlientListResponse> cache)
        {
            _context = klinikaContext;
            _passwordRepository = password;
            _configuration = config;
            _emailSender = sender;
            _cache = cache;
        }

        public async Task<int> Handle(CreateKlientCommand req, CancellationToken cancellationToken)
        {
            if (!req.request.Haslo.Equals(req.request.Haslo2))
            {
                throw new Exception("Incorrect password");
            }
            if (_context.Osobas.Where(x => x.NazwaUzytkownika.Equals(req.request.NazwaUzytkownika)).Any())
            {
                throw new Exception("Not unique");
            }

            byte[] salt = _passwordRepository.GenerateSalt();
            string hashedPassword = _passwordRepository.HashPassword(salt, req.request.Haslo, int.Parse(_configuration["PasswordIterations"]));
            string saltBase64 = Convert.ToBase64String(salt);

            var query = "exec DodajKlienta @imie, @nazwisko, @numerTel, @email, @login, @haslo, @salt";

            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("KlinikaDatabase"));
            await connection.OpenAsync();
            //SqlTransaction trans = connection.BeginTransaction();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@imie", req.request.Imie);
            command.Parameters.AddWithValue("@nazwisko", req.request.Nazwisko);
            command.Parameters.AddWithValue("@numerTel", req.request.NumerTelefonu);
            command.Parameters.AddWithValue("@email", req.request.Email);
            command.Parameters.AddWithValue("@login", req.request.NazwaUzytkownika);
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