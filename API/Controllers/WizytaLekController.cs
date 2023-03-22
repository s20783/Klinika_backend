using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System;
using Application.WizytaLeki.Queries;
using Application.WizytaLeki.Commands;

namespace PRO_API.Controllers
{
    public class WizytaLekController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz,klient")]
        [HttpGet("{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaLekList(string ID_wizyta, CancellationToken token)
        {
            return Ok(await Mediator.Send(new WizytaLekListQuery
            {
                ID_wizyta = ID_wizyta
            }, token));
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpPost("{ID_wizyta}/{ID_lek}")]
        public async Task<IActionResult> AddWizytaLek(string ID_wizyta, string ID_lek, int Ilosc, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new AddWizytaLekCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_Lek = ID_lek,
                    Ilosc = Ilosc
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [Authorize(Roles = "admin,weterynarz")]
        [HttpDelete("{ID_wizyta}/{ID_lek}")]
        public async Task<IActionResult> RemoveWizytaLek(string ID_wizyta, string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new RemoveWizytaLekCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_lek = ID_lek
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }
    }
}