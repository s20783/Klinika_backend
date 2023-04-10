using Application.WizytaUslugi.Commands;
using Application.WizytaUslugi.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class WizytaUslugaController : ApiControllerBase
    {
        [Authorize]
        [HttpGet("{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaUslugaList(string ID_wizyta, CancellationToken token)
        {
            return Ok(await Mediator.Send(new WizytaUslugaQuery
            {
                ID_wizyta = ID_wizyta
            }, token));
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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
