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
    public class ClientController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetKlientList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new ClientListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            return Ok(await Mediator.Send(new ClientListAllQuery(), token));
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlientById(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ClientQuery
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
        public async Task<IActionResult> AddKlient(ClientRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateClientCommand
                {
                    request = request
                }, token));
            } 
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost("Klinika")]
        public async Task<IActionResult> AddKlient(ClientAdminRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateClientAdminCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("admin/{ID_osoba}")]
        public async Task<IActionResult> DeleteKlientByAdmin(string ID_osoba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteClientCommand
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