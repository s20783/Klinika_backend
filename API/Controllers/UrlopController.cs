using Application.DTO.Requests;
using Application.Urlopy.Commands;
using Application.Urlopy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class UrlopController : ApiControllerBase
    {
      //  [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUrlopList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new UrlopListQuery
            {

            }, token));
        }

       // [Authorize(Roles = "weterynarz")]
        [HttpGet("moje_urlopy")]
        public async Task<IActionResult> GetUrlopList2(CancellationToken token)
        {
            return Ok(await Mediator.Send(new UrlopWeterynarzQuery
            {
                ID_weterynarz = GetUserId()
            }, token));
        }

     //   [Authorize(Roles = "admin")]
        [HttpGet("{ID_weterynarz}")]
        public async Task<IActionResult> GetWeterynarzUrlopList(string ID_weterynarz, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UrlopWeterynarzQuery
                {
                    ID_weterynarz = ID_weterynarz
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

       // [Authorize(Roles = "admin")]
        [HttpGet("details/{ID_urlop}")]
        public async Task<IActionResult> GetUrlopDetails(string ID_urlop, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UrlopDetailsQuery
                {
                    ID_urlop = ID_urlop
                }, token));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

       // [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddUrlop(UrlopRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateUrlopCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpPut("{ID_urlop}")]
        public async Task<IActionResult> UpdateUrlop(string ID_urlop, UrlopRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateUrlopCommand
                {
                    ID_urlop = ID_urlop,
                    request = request
                }, token);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }

            return NoContent();
        }

       // [Authorize(Roles = "admin")]
        [HttpDelete("{ID_urlop}")]
        public async Task<IActionResult> DeleteUrlop(string ID_urlop, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteUrlopCommand
                {
                    ID_urlop = ID_urlop
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