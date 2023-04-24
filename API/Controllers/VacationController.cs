using Application.DTO.Requests;
using Application.Urlopy.Commands;
using Application.Urlopy.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class VacationController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUrlopList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new VacationListQuery
            {

            }, token));
        }

        [AuthorizeRoles(RoleEnum.Weterynarz)]
        [HttpGet("moje_urlopy")]
        public async Task<IActionResult> GetUrlopList2(CancellationToken token)
        {
            return Ok(await Mediator.Send(new VacationVetQuery
            {
                ID_weterynarz = GetUserId()
            }, token));
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("{ID_weterynarz}")]
        public async Task<IActionResult> GetWeterynarzUrlopList(string ID_weterynarz, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VacationVetQuery
                {
                    ID_weterynarz = ID_weterynarz
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("details/{ID_urlop}")]
        public async Task<IActionResult> GetUrlopDetails(string ID_urlop, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VacationDetailsQuery
                {
                    ID_urlop = ID_urlop
                }, token));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddUrlop(VacationRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateVacationCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPut("{ID_urlop}")]
        public async Task<IActionResult> UpdateUrlop(string ID_urlop, VacationRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateVacationCommand
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

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("{ID_urlop}")]
        public async Task<IActionResult> DeleteUrlop(string ID_urlop, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteVacationCommand
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