using Application.DTO.Request;
using Application.Specjalizacje.Commands;
using Application.Specjalizacje.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SpecjalizacjaController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetSpecjalizacjaList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new SpecjalizacjaListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("details/{ID_specjalizacja}")]
        public async Task<IActionResult> GetSpecjalizacjaById(string ID_specjalizacja, CancellationToken token)
        {
            try 
            {
                return Ok(await Mediator.Send(new SpecjalizacjaDetailsQuery
                {
                    ID_specjalizacja = ID_specjalizacja
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddSpecjalizacja(SpecjalizacjaRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateSpecjalizacjaCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPut("{ID_specjalizacja}")]
        public async Task<IActionResult> UpdateSpecjalizacja(string ID_specjalizacja, SpecjalizacjaRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateSpecjalizacjaCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpDelete("{ID_specjalizacja}")]
        public async Task<IActionResult> DeleteSpecjalizacja(string ID_specjalizacja, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteSpecjalizacjaCommand
                {
                    ID_specjalizacja = ID_specjalizacja
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