using Application.DTO.Requests;
using Application.Szczepienia.Commands;
using Application.Szczepienia.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class VaccinationController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz, RoleEnum.Klient)]
        [HttpGet("{ID_pacjent}")]
        public async Task<IActionResult> GetSzczepienie(string ID_pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VaccinationPacjentQuery
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
        [HttpGet("details/{ID_szczepienie}")]
        public async Task<IActionResult> GetSzczepienieDetails(string ID_szczepienie, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VaccinationDetailsQuery
                {
                    ID_szczepienie = ID_szczepienie
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddSzczepienie(VaccinationRequest szczepienieRequest, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new CreateVaccinationCommand
                {
                    request = szczepienieRequest
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("{ID_szczepienie}")]
        public async Task<IActionResult> UpdateSzczepienie(VaccinationRequest szczepienieRequest, string ID_szczepienie, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateVaccinationCommand
                {
                    ID_szczepienie = ID_szczepienie,
                    request = szczepienieRequest
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_szczepienie}")]
        public async Task<IActionResult> DeleteSzczepienie(string ID_szczepienie, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteVaccinationCommand
                {
                    ID_szczepienie = ID_szczepienie
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