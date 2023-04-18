using Application.DTO;
using Application.DTO.Requests;
using Application.Klienci.Commands;
using Application.Klienci.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class KlientController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetKlientList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new KlientListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            return Ok(await Mediator.Send(new KlientListAllQuery(), token));
        }


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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

        [AuthorizeRoles(RolaEnum.Admin)]
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