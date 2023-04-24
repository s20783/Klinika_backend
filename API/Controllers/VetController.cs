using Application.DTO;
using Application.Weterynarze.Commands;
using Application.Weterynarze.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class VetController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetWeterynarzList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new VetListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllWeterynarz(CancellationToken token)
        {
            return Ok(await Mediator.Send(new VetListAllQuery(), token));
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetWeterynarzById(string ID_osoba, CancellationToken token)
        {
            return Ok(await Mediator.Send(new VetDetailsQuery
            {
                ID_osoba = ID_osoba
            }, token));
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddWeterynarz(VetCreateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateVetCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateWeterynarz(string ID_osoba, VetUpdateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateVetCommand
                {
                    ID_osoba = ID_osoba,
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteWeterynarz(string ID_osoba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteVetCommand
                {
                    ID_osoba = ID_osoba
                }, token);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
            
            return NoContent();
        }
    }
}