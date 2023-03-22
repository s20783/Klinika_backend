using Application.DTO.Requests;
using Application.Uslugi.Commands;
using Application.Uslugi.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class UslugaController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetUslugaList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new UslugaListQuery
            {

            }, token));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("details/{ID_usluga}")]
        public async Task<IActionResult> GetUslugaDetails(string ID_usluga, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UslugaDetailsQuery
                {
                    ID_usluga = ID_usluga
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaUslugaList(string ID_wizyta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UslugaWizytaListQuery
                {
                    ID_wizyta = ID_wizyta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("pacjent/{ID_pacjent}")]
        public async Task<IActionResult> GetPacjentUslugaList(string ID_pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UslugaPacjentListQuery
                {
                    ID_pacjent = ID_pacjent
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddUsluga(UslugaRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateUslugaCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_usluga}")]
        public async Task<IActionResult> UpdateUsluga(string ID_usluga, UslugaRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateUslugaCommand
                {
                    ID_usluga = ID_usluga,
                    request = request
                }, token);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{ID_usluga}")]
        public async Task<IActionResult> DeleteUsluga(string ID_usluga, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteUslugaCommand
                {
                    ID_usluga = ID_usluga
                }, token);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}