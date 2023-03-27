using Application.DTO.Request;
using Application.Pacjenci.Commands;
using Application.Pacjenci.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class PacjentController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetPacjentList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new PacjentListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("klient/{ID_osoba}")]
        public async Task<IActionResult> GetKlientPacjentList(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PacjentKlientListQuery
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Klient)]
        [HttpGet("klient")]
        public async Task<IActionResult> GetKlientPacjentList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PacjentKlientListQuery
                {
                    ID_osoba = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("details/{ID_pacjent}")]
        public async Task<IActionResult> GetPacjentById(string ID_pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PacjentDetailsQuery
                {
                    ID_pacjent = ID_pacjent
                }, token));
            } 
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddPacjent(PacjentCreateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreatePacjentCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPut("{ID_Pacjent}")]
        public async Task<IActionResult> UpdatePacjent(string ID_Pacjent, PacjentCreateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdatePacjentCommand
                {
                    request = request,
                    ID_pacjent = ID_Pacjent
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpDelete("{ID_Pacjent}")]
        public async Task<IActionResult> DeletePacjent(string ID_Pacjent, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeletePacjentCommand
                {
                    ID_Pacjent = ID_Pacjent
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