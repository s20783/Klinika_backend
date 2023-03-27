using Application.DTO.Requests;
using Application.Uslugi.Commands;
using Application.Uslugi.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class UslugaController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetUslugaList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new UslugaListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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

        [AuthorizeRoles(RolaEnum.Admin)]
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

        [AuthorizeRoles(RolaEnum.Admin)]
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

        [AuthorizeRoles(RolaEnum.Admin)]
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