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
    public class ScheduleController : ApiControllerBase
    {
        //ustawia harmonogramy (według godzin pracy) weterynarzom na tydzień do przodu względem ostatniego harmonogramu w systemie
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost("auto")]
        public async Task<IActionResult> AddHarmonogramsForAWeek(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new AutoCreateScheduleCommand
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
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPut("auto")]
        public async Task<IActionResult> AddWeterynarzHarmonograms(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new AutoUpdateScheduleCommand
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
                return Ok(await Mediator.Send(new ScheduleClientQuery
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
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("klinika/{ID_osoba}")]
        public async Task<IActionResult> GetKlinikaAdminHarmonogram(string ID_osoba, DateTime date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ScheduleAdminByIDQuery
                {
                    ID_osoba = ID_osoba,
                    Date = date,
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("klinika")]
        public async Task<IActionResult> GetKlinikaHarmonogram(DateTime date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ScheduleAdminQuery
                {
                    Date = date
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Weterynarz)]
        [HttpGet("moj_harmonogram")]
        public async Task<IActionResult> GetWeterynarzHarmonogram(DateTime date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ScheduleVetQuery
                {
                    ID_osoba = GetUserId(),
                    Date = date
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}