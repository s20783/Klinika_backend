using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
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

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        /*private readonly IConfiguration configuration;
        private readonly IHashids hashids;
        private readonly KlinikaContext context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TestController> logger;
        public TestController(IEmailSender emailSender, IConfiguration config, IHashids ihashids, KlinikaContext klinikaContext, ILogger<TestController> _logger)
        {
            _emailSender = emailSender;
            configuration = config;
            hashids = ihashids;
            context = klinikaContext;
            logger = _logger;
        }
        

        [HttpGet]
        public IActionResult TestHaslo(string plainPassword, string salt)
        {
            try
            {
                string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: plainPassword,
                    salt: Convert.FromBase64String(salt),
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 50000,
                    numBytesRequested: 512 / 8));

                return Ok(hashedPassword);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("hashid/{id}")]
        public IActionResult GetHashedID(int id)
        {
            return Ok(hashids.Encode(id));
        }

        [HttpGet("time")]
        public IActionResult GetHashedID(DateTime dateTime, DateTimeOffset dateTimeOffset)
        {
            return Ok(new
            {
                utc = dateTime.ToUniversalTime(),
                local = dateTime.ToLocalTime(),
                offset = dateTimeOffset,
                offset2 = dateTimeOffset.ToUniversalTime(),
            });
        }

        [HttpPost("email/haslo")]
        public async Task<IActionResult> SendTestEmail()
        {
            try
            {
                await _emailSender.SendHasloEmail("to@example.com", "**password**");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/umowWizyte")]
        public async Task<IActionResult> SendTestEmail2()
        {
            try
            {
                await _emailSender.SendUmowWizytaEmail("to@example.com", DateTime.Now, "Zbigniew Nowak");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/anulujWizyte")]
        public async Task<IActionResult> SendTestEmail3()
        {
            try
            {
                await _emailSender.SendAnulujWizyteEmail("to@example.com", DateTime.Now);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("email/createAccount")]
        public async Task<IActionResult> SendTestEmail4()
        {
            try
            {
                await _emailSender.SendCreateAccountEmail("to@example.com");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }*/
    }
}