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
    public class VetSpecializationController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_weterynarz}")]
        public async Task<IActionResult> GetWeterynarzSpecjalizacja(string ID_weterynarz, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VetSpecializationListQuery
                {
                    ID_weterynarz = ID_weterynarz
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost("{ID_specjalizacja}/{ID_weterynarz}")]
        public async Task<IActionResult> AddSpecjalizacjaToWeterynarz(string ID_specjalizacja, string ID_weterynarz, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddVetSpecializationCommand
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

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("{ID_specjalizacja}/{ID_weterynarz}")]
        public async Task<IActionResult> RemoveSpecjalizacjaFromWeterynarz(string ID_specjalizacja, string ID_weterynarz, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveVetSpecializationCommand
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