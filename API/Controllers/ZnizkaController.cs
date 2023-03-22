using Application.Znizki.Commands;
using Application.Znizki.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class ZnizkaController : ApiControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetZnizkaList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ZnizkaListQuery
                {

                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("details/{ID_znizka}")]
        public async Task<IActionResult> GetZnizkaDetails(string ID_znizka, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ZnizkaDetailsQuery
                {
                    ID_znizka = ID_znizka
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{ID_znizka}")]
        public async Task<IActionResult> UpdateZnizka(string ID_znizka, string ZnizkaNazwa, decimal ProcentZnizki, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateZnizkaCommand
                {
                    ID_znizka = ID_znizka,
                    Nazwa = ZnizkaNazwa,
                    Procent = ProcentZnizki
                }, token);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}