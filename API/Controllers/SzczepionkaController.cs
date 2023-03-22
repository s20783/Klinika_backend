using Application.DTO.Requests;
using Application.Szczepionki.Commands;
using Application.Szczepionki.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SzczepionkaController : ApiControllerBase
    {
        //[Authorize(Roles = "admin,weterynarz")]
        [HttpGet]
        public async Task<IActionResult> GetSzczepionkaList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepionkaListQuery
                {

                }, token));
            } catch (Exception)
            {
                return NotFound();
            }
        }


        //[Authorize(Roles = "admin,weterynarz")]
        [HttpGet("details/{ID_szczepionka}")]
        public async Task<IActionResult> GetSzczepionkaDetails(string ID_szczepionka, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepionkaDetailsQuery
                {
                    ID_szczepionka = ID_szczepionka
                }, token));
            } catch (Exception)
            {
                return NotFound();
            }
        }


        //[Authorize(Roles = "admin,weterynarz")]
        [HttpPost]
        public async Task<IActionResult> AddSzczepionka(SzczepionkaRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new CreateSzczepionkaCommand
                {
                    request = request
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }


        //[Authorize(Roles = "admin,weterynarz")]
        [HttpPut("{ID_szczepionka}")]
        public async Task<IActionResult> UpdateSzczepionka(string ID_szczepionka, SzczepionkaRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateSzczepionkaCommand
                {
                    ID_szczepionka = ID_szczepionka,
                    request = request
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }


        //[Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_szczepionka}")]
        public async Task<IActionResult> DeleteSzczepionka(string ID_szczepionka, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteSzczepionkaCommand
                {
                    ID_szczepionka = ID_szczepionka
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