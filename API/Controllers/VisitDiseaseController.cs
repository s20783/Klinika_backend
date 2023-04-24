using Application.WizytaChoroby.Commands;
using Application.WizytaChoroby.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PRO_API.Common;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class VisitDiseaseController : ApiControllerBase
    {
        [Authorize]
        [HttpGet("{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaChorobaList(string ID_wizyta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VisitDiseaseQuery
                {
                    ID_wizyta = ID_wizyta
                }, token));
            } 
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost("{ID_wizyta}/{ID_choroba}")]
        public async Task<IActionResult> AddWizytaChoroba(string ID_wizyta, string ID_choroba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddVisitDiseaseCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_choroba = ID_choroba
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_wizyta}/{ID_choroba}")]
        public async Task<IActionResult> RemoveWizytaChoroba(string ID_wizyta, string ID_choroba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveVisitDiseaseCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_choroba = ID_choroba
                }, token);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return NoContent();
        }
    }
}