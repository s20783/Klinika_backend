using Application.DTO.Requests;
using Application.Szczepienia.Commands;
using Application.Szczepienia.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SzczepienieController : ApiControllerBase
    {
        //[Authorize(Roles = "weterynarz,admin")]
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

        //[Authorize(Roles = "weterynarz,admin")]
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

        //[Authorize(Roles = "weterynarz,admin")]
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

        //[Authorize(Roles = "weterynarz,admin")]
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

        [Authorize(Roles = "weterynarz,admin")]
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