using Application.DTO.Request;
using Application.LekiWMagazynie.Commands;
using Application.LekiWMagazynie.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class LekWMagazynieController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_stan_leku}")]
        public async Task<IActionResult> GetLekWMagazynieById(string ID_stan_leku, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new StanLekuQuery
                {
                    ID_stan_leku = ID_stan_leku
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost("{ID_lek}")]
        public async Task<IActionResult> AddStanLeku(string ID_lek, StanLekuRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateStanLekuCommand
                {
                    ID_lek = ID_lek,
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpPut("{ID_stan_leku}")]
        public async Task<IActionResult> UpdateStanLeku(string ID_stan_leku, StanLekuRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateStanLekuCommand
                {
                    ID_stan_leku = ID_stan_leku,
                    request = request
                }, token);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }


        [Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_stan_leku}")]
        public async Task<IActionResult> DeleteStanLeku(string ID_stan_leku, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteStanLekuCommand
                {
                    ID_stan_leku = ID_stan_leku
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