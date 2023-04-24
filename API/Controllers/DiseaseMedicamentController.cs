using Application.ChorobaLeki.Commands;
using Application.ChorobaLeki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class DiseaseMedicamentController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetChorobaLek(string ID_lek, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DiseaseMedicamentListQuery
                {
                    ID_lek = ID_lek
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost("{ID_choroba}/{ID_lek}")]
        public async Task<IActionResult> AddChorobaToLek(string ID_choroba, string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddDiseaseMedicamentCommand
                {
                    ID_choroba = ID_choroba,
                    ID_lek = ID_lek
                }, token);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_choroba}/{ID_lek}")]
        public async Task<IActionResult> RemoveChorobaFromLek(string ID_choroba, string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveDiseaseMedicamentCommand
                {
                    ID_choroba = ID_choroba,
                    ID_lek = ID_lek
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