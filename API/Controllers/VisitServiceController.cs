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
    public class VisitServiceController : ApiControllerBase
    {
        [Authorize]
        [HttpGet("{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaUslugaList(string ID_wizyta, CancellationToken token)
        {
            return Ok(await Mediator.Send(new VisitServiceListQuery
            {
                ID_wizyta = ID_wizyta
            }, token));
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost("{ID_wizyta}/{ID_usluga}")]
        public async Task<IActionResult> AddWizytaUsluga(string ID_wizyta, string ID_usluga, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddVisitServiceCommand
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

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("accept/{ID_wizyta}")]
        public async Task<IActionResult> AcceptWizytaUsluga(string ID_wizyta, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AcceptVisitServicesCommand
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

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_wizyta}/{ID_usluga}")]
        public async Task<IActionResult> RemoveWizytaUsluga(string ID_wizyta, string ID_usluga, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveVisitServiceCommand
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
