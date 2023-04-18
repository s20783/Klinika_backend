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
    public class WeterynarzController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetWeterynarzList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new WeterynarzListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }


        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllWeterynarz(CancellationToken token)
        {
            return Ok(await Mediator.Send(new WeterynarzListAllQuery(), token));
        }


        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetWeterynarzById(string ID_osoba, CancellationToken token)
        {
            return Ok(await Mediator.Send(new WeterynarzDetailsQuery
            {
                ID_osoba = ID_osoba
            }, token));
        }


        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddWeterynarz(WeterynarzCreateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateWeterynarzCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateWeterynarz(string ID_osoba, WeterynarzUpdateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateWeterynarzCommand
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


        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteWeterynarz(string ID_osoba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteWeterynarzCommand
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