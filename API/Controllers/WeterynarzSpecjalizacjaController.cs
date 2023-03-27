using Application.WeterynarzSpecjalizacje.Commands;
using Application.WeterynarzSpecjalizacje.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class WeterynarzSpecjalizacjaController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("{ID_weterynarz}")]
        public async Task<IActionResult> GetWeterynarzSpecjalizacja(string ID_weterynarz, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new WeterynarzSpecjalizacjaListQuery
                {
                    ID_weterynarz = ID_weterynarz
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost("{ID_specjalizacja}/{ID_weterynarz}")]
        public async Task<IActionResult> AddSpecjalizacjaToWeterynarz(string ID_specjalizacja, string ID_weterynarz, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddSpecjalizacjaWeterynarzCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    ID_weterynarz = ID_weterynarz
                }, token);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return NoContent();
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpDelete("{ID_specjalizacja}/{ID_weterynarz}")]
        public async Task<IActionResult> RemoveSpecjalizacjaFromWeterynarz(string ID_specjalizacja, string ID_weterynarz, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveSpecjalizacjaWeterynarzCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    ID_weterynarz = ID_weterynarz
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