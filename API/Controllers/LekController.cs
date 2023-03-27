using Application.DTO.Requests;
using Application.Leki.Commands;
using Application.Leki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class LekController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetLekList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new LekListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("lekOnly")]
        public async Task<IActionResult> GetLekOnlyList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new LekOnlyListQuery
            {

            }, token));
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
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