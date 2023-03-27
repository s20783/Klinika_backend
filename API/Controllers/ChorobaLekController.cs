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
    public class ChorobaLekController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetChorobaLek(string ID_lek, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ChorobaLekListQuery
                {
                    ID_lek = ID_lek
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPost("{ID_choroba}/{ID_lek}")]
        public async Task<IActionResult> AddChorobaToLek(string ID_choroba, string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddChorobaLekCommand
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

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpDelete("{ID_choroba}/{ID_lek}")]
        public async Task<IActionResult> RemoveChorobaFromLek(string ID_choroba, string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveChorobaLekCommand
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