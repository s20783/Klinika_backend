using Application.Choroby.Commands;
using Application.Choroby.Queries;
using Application.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class ChorobaController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetChorobaList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new ChorobaListQuery
            {

            }, token));
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_Choroba}")]
        public async Task<IActionResult> GetChorobaDetails(string ID_Choroba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ChorobaDetailsQuery
                {
                    ID_Choroba = ID_Choroba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost]
        public async Task<IActionResult> AddChoroba(ChorobaRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateChorobaCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(new 
                { 
                    message = e.Message
                });
            }
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPut("{ID_Choroba}")]
        public async Task<IActionResult> UpdateChoroba(string ID_Choroba, ChorobaRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateChorobaCommand
                {
                    ID_Choroba = ID_Choroba,
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message
                });
            }

        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_Choroba}")]
        public async Task<IActionResult> DeleteChoroba(string ID_Choroba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteChorobaCommand
                {
                    ID_Choroba = ID_Choroba
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