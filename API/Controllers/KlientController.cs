using Application.DTO;
using Application.DTO.Requests;
using Application.Klienci.Commands;
using Application.Klienci.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class KlientController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetKlientList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new KlientListQuery
            {

            }, token));
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlientById(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new KlientQuery
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddKlient(KlientCreateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateKlientCommand
                {
                    request = request
                }, token));
            } 
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPost("Klinika")]
        public async Task<IActionResult> AddKlient(KlientCreateKlinikaRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateKlientKlinikaCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = "klient")]
        [HttpDelete]
        public async Task<IActionResult> DeleteKlient(CancellationToken token)
        {
            try
            {
                if (isKlient())
                {
                    await Mediator.Send(new DeleteKlientCommand
                    {
                        ID_osoba = GetUserId()
                    }, token);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{ID_osoba}")]
        public async Task<IActionResult> DeleteKlientByAdmin(string ID_osoba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteKlientCommand
                {
                    ID_osoba = ID_osoba
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            
            return NoContent();
        }
    }
}