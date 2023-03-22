using Application.DTO.Request;
using Application.Specjalizacje.Commands;
using Application.Specjalizacje.Queries;
using Application.WeterynarzSpecjalizacje.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SpecjalizacjaController : ApiControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetSpecjalizacjaList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new SpecjalizacjaListQuery
            {

            }));
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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