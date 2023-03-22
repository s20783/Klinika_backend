using Application.DTO;
using Application.Weterynarze.Commands;
using Application.Weterynarze.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class WeterynarzController : ApiControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetWeterynarzList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new WeterynarzListQuery
            {
                
            }, token));
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetWeterynarzById(string ID_osoba, CancellationToken token)
        {
            return Ok(await Mediator.Send(new WeterynarzQuery
            {
                ID_osoba = ID_osoba
            }, token));
        }


        [Authorize(Roles = "admin")]
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
                return NotFound(new
                {
                    message = e.Message
                });
            }
        }


        [Authorize(Roles = "admin")]
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
                return BadRequest(e);
            }
        }


        [Authorize(Roles = "admin")]
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
                return NotFound(e);
            }
            
            return NoContent();
        }
    }
}