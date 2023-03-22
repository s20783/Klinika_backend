﻿using Application.Uslugi.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using Application.WizytaUslugi.Queries;
using Application.Wizyty.Queries;
using System;
using Application.WizytaUslugi.Commands;

namespace PRO_API.Controllers
{
    public class WizytaUslugaController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz,klient")]
        [HttpGet("{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaUslugaList(string ID_wizyta, CancellationToken token)
        {
            if(isAdmin() || isWeterynarz())
            {
                return Ok(await Mediator.Send(new WizytaUslugaKlinikaQuery
                {
                    ID_wizyta = ID_wizyta
                }, token));
            }

            return Ok(await Mediator.Send(new WizytaUslugaKlientQuery
            {
                ID_wizyta = ID_wizyta
            }, token));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost("{ID_wizyta}/{ID_usluga}")]
        public async Task<IActionResult> AddWizytaUsluga(string ID_wizyta, string ID_usluga, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddWizytaUslugaCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_usluga = ID_usluga
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpPut("accept/{ID_wizyta}")]
        public async Task<IActionResult> AcceptWizytaUsluga(string ID_wizyta, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AcceptWizytaUslugaCommand
                {
                    ID_wizyta = ID_wizyta
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_wizyta}/{ID_usluga}")]
        public async Task<IActionResult> RemoveWizytaUsluga(string ID_wizyta, string ID_usluga, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveWizytaUslugaCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_usluga = ID_usluga
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }
    }
}
