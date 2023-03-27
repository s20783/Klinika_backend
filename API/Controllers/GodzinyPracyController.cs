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
    public class GodzinyPracyController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetGodzinyPracyDzien(string ID_osoba, int Dzien, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new GodzinyPracyDzienQuery
                {
                    ID_osoba = ID_osoba,
                    Dzien = Dzien
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Weterynarz)]
        [HttpGet("moje_godziny")]
        public async Task<IActionResult> GetGodzinyPracyWeterynarz(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new GodzinyPracyQuery
                {
                    ID_osoba = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("list/{ID_osoba}")]
        public async Task<IActionResult> GetGodzinyPracy(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new GodzinyPracyQuery
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost("list/{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracyList(string ID_osoba, List<GodzinyPracyRequest> requests, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = requests
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost("default/{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracyList(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateDefaultGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost("{ID_osoba}")]
        public async Task<IActionResult> AddGodzinyPracy(string ID_osoba, GodzinyPracyRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = new List<GodzinyPracyRequest>() { request }
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPut("list/{ID_osoba}")]
        public async Task<IActionResult> UpdateGodzinyPracy(string ID_osoba, List<GodzinyPracyRequest> requests, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = requests
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPut("{ID_osoba}")]
        public async Task<IActionResult> UpdateGodzinyPracy(string ID_osoba, GodzinyPracyRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    requestList = new List<GodzinyPracyRequest>() { request }
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpDelete("{ID_osoba}")]
        public async Task<IActionResult> DeleteGodzinyPracy(string ID_osoba, int dzien, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteGodzinyPracyCommand
                {
                    ID_osoba = ID_osoba,
                    dzien = dzien
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