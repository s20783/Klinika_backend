using Application.Harmonogramy.Commands;
using Application.Harmonogramy.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class HarmonogramController : ApiControllerBase
    {
        //ustawia harmonogramy (według godzin pracy) weterynarzom na tydzień do przodu względem ostatniego harmonogramu w systemie
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPost("auto")]
        public async Task<IActionResult> AddHarmonogramsForAWeek(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new AutoCreateHarmonogramCommand
                {

                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //ustawia harmonogramy weterynarzom od dzisiejszej daty do daty ostatniego harmonogramu w systemie
        //(może być użyty w przypadku nowych weterynarzy)
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpPut("auto")]
        public async Task<IActionResult> AddWeterynarzHarmonograms(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new AutoUpdateHarmonogramCommand
                {

                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //klient umawia wizytę albo pracownik kliniki umówia wizytę na prośbę klienta
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetHarmonogram(DateTime date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new HarmonogramKlientQuery
                {
                    Date = date
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //admin wyświetla harmonogram weterynarza
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("klinika/{ID_osoba}")]
        public async Task<IActionResult> GetKlinikaAdminHarmonogram(string ID_osoba, DateTime Date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new HarmonogramAdminByIDQuery
                {
                    ID_osoba = ID_osoba,
                    Date = Date,
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("klinika")]
        public async Task<IActionResult> GetKlinikaHarmonogram(DateTime date, CancellationToken token)
        {
            try
            {
                if (isWeterynarz())
                {
                    return Ok(await Mediator.Send(new HarmonogramWeterynarzQuery
                    {
                        ID_osoba = GetUserId(),
                        Date = date
                    }, token));
                }

                return Ok(await Mediator.Send(new HarmonogramAdminQuery
                {
                    Date = date
                }));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}