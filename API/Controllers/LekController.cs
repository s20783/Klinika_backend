using Application.DTO.Requests;
using Application.Leki.Commands;
using Application.Leki.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class LekController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetLekList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new LekListQuery
            {

            }, token));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("lekOnly")]
        public async Task<IActionResult> GetLekOnlyList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new LekOnlyListQuery
            {

            }, token));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetLekById(string ID_lek, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new LekQuery
                {
                    ID_lek = ID_lek
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost]
        public async Task<IActionResult> AddLek(LekRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateLekCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPut("{ID_lek}")]
        public async Task<IActionResult> UpdateLek(string ID_lek, LekRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateLekCommand
                {
                    ID_lek = ID_lek,
                    request = request
                }, token);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_lek}")]
        public async Task<IActionResult> DeleteLek(string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteLekCommand
                {
                    ID_lek = ID_lek
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