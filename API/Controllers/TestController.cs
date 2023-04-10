using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Threading;
using Microsoft.Extensions.Logging;
using Application.DTO.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PRO_API.Common;
using Domain.Enums;
using System.Collections.Generic;
using Infrastructure;
using Org.BouncyCastle.Ocsp;

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHashids _hash;
        private readonly KlinikaContext context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TestController> logger;
        public TestController(IEmailSender emailSender, IConfiguration config, IHashids ihashids, KlinikaContext klinikaContext, ILogger<TestController> _logger)
        {
            _emailSender = emailSender;
            configuration = config;
            _hash = ihashids;
            context = klinikaContext;
            logger = _logger;
        }
        

        [HttpGet]
        public IActionResult TestHaslo()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}