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
    public class PatientController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetPacjentList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new PatientListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("klient/{ID_osoba}")]
        public async Task<IActionResult> GetKlientPacjentList(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PatientKlientListQuery
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Klient)]
        [HttpGet("klient")]
        public async Task<IActionResult> GetKlientPacjentList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PatientKlientListQuery
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
                return Ok(await Mediator.Send(new PatientDetailsQuery
                {
                    ID_pacjent = ID_pacjent
                }, token));
            } 
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddPacjent(PatientRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreatePatientCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("{ID_Pacjent}")]
        public async Task<IActionResult> UpdatePacjent(string ID_Pacjent, PatientRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdatePatientCommand
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

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_Pacjent}")]
        public async Task<IActionResult> DeletePacjent(string ID_Pacjent, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeletePatientCommand
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