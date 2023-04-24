using Application.DTO.Requests;
using Application.GodzinaPracy.Commands;
using Application.GodzinaPracy.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class WorkingHoursController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Weterynarz)]
        [HttpGet("moje_godziny")]
        public async Task<IActionResult> GetGodzinyPracyWeterynarz(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new WorkingHoursListQuery
                {
                    ID_osoba = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetGodzinyPracy(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new WorkingHoursListQuery
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost("default/{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracyList(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateDefaultWorkingHoursCommand
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost("{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracy(string ID_osoba, WorkingHoursRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateWorkingHoursCommand
                {
                    ID_osoba = ID_osoba,
                    Request = request 
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateGodzinyPracy(string ID_osoba, WorkingHoursRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateWorkingHoursCommand
                {
                    ID_osoba = ID_osoba,
                    Request = request 
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteGodzinyPracy(string ID_osoba, int day, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteWorkingHoursCommand
                {
                    ID_osoba = ID_osoba,
                    Day = day
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