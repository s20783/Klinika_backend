using Application.DTO.Requests;
using Application.Recepty.Commands;
using Application.Recepty.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class ReceptaController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_Klient}")]
        public async Task<IActionResult> GetReceptaKlientList(string ID_Klient, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaKlientQuery
                {
                    ID_klient = ID_Klient
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient")]
        [HttpGet("moje_recepty")]
        public async Task<IActionResult> GetReceptaKlientList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaKlientQuery
                {
                    ID_klient = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet("details/{ID_Recepta}")]
        public async Task<IActionResult> GetReceptaDetails(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaDetailsQuery
                {
                    ID_recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet("pacjent/{ID_Pacjent}")]
        public async Task<IActionResult> GetReceptaPacjentList(string ID_Pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ReceptaPacjentQuery
                {
                    ID_pacjent = ID_Pacjent
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPost]
        public async Task<IActionResult> AddRecepta(string ID_Wizyta, string Zalecenia, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateReceptaCommand
                {
                    ID_wizyta = ID_Wizyta,
                    Zalecenia = Zalecenia,
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPut("{ID_Recepta}")]
        public async Task<IActionResult> UpdateRecepta(string ID_Recepta, string Zalecenia, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateReceptaCommand
                {
                    ID_recepta = ID_Recepta,
                    Zalecenia = Zalecenia
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpDelete("{ID_Recepta}")]
        public async Task<IActionResult> DeleteRecepta(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteReceptaCommand
                {
                    ID_recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}