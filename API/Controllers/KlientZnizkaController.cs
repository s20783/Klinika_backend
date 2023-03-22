using Application.KlientZnizki.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class KlientZnizkaController : ApiControllerBase
    {
        [Authorize(Roles = "admin,weterynarz")]
        [HttpGet("{ID_osoba}")]
        public async Task<IActionResult> GetKlientZnizka(string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new KlientZnizkaListQuery
                {
                    ID_klient = ID_osoba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient")]
        [HttpGet("moje_znizki")]
        public async Task<IActionResult> GetKlientZnizka(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new KlientZnizkaListQuery
                {
                    ID_klient = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}