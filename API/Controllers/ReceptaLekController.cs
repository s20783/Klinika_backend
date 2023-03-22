using Application.ReceptaLeki.Commands;
using Application.ReceptaLeki.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class ReceptaLekController : ApiControllerBase
    {
        //[Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet("{ID_Recepta}")]
        public async Task<IActionResult> GetReceptaLek(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaLekQuery
                {
                    ID_Recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPost]
        public async Task<IActionResult> AddReceptaLek(string ID_Recepta, string ID_Lek, int Ilosc, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateReceptaLekCommand
                {
                    ID_Recepta = ID_Recepta,
                    ID_Lek = ID_Lek,
                    Ilosc = Ilosc
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpDelete("{ID_Recepta}/{ID_Lek}")]
        public async Task<IActionResult> DeleteReceptaLek(string ID_Recepta, string ID_Lek, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteReceptaLekCommand
                {
                    ID_Recepta = ID_Recepta,
                    ID_Lek = ID_Lek
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}