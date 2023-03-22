using Application.DTO.Requests;
using Application.GodzinaPracy.Commands;
using Application.GodzinaPracy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class GodzinyPracyController : ApiControllerBase
    {
        //[Authorize(Roles = "admin")]
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

        [Authorize(Roles = "weterynarz")]
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

        //[Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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