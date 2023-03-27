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
using PRO_API.Common;
using Domain.Enums;
using System.Collections.Generic;

namespace PRO_API.Controllers
{
    public class TestController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
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
        public IActionResult TestHaslo()
        {
            try
            {
                var roles = new List<RolaEnum>
                {
                    RolaEnum.Klient,
                    RolaEnum.Weterynarz
                };
                var Roles = string.Join(",", Array.ConvertAll(roles.ToArray(), x => Enum.GetName(typeof(RolaEnum), x)));

                return Ok(Roles);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

    }
}