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
    public class SzczepienieController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz, RolaEnum.Klient)]
        [HttpGet("{ID_pacjent}")]
        public async Task<IActionResult> GetSzczepienie(string ID_pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepieniePacjentQuery
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
        [HttpGet("details/{ID_szczepienie}")]
        public async Task<IActionResult> GetSzczepienieDetails(string ID_szczepienie, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepienieDetailsQuery
                {
                    ID_szczepienie = ID_szczepienie
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddSzczepienie(SzczepienieRequest szczepienieRequest, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new CreateSzczepienieCommand
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

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPut("{ID_szczepienie}")]
        public async Task<IActionResult> UpdateSzczepienie(SzczepienieRequest szczepienieRequest, string ID_szczepienie, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateSzczepienieCommand
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

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpDelete("{ID_szczepienie}")]
        public async Task<IActionResult> DeleteSzczepienie(string ID_szczepienie, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteSzczepienieCommand
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