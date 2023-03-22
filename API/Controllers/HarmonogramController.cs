using Application.Harmonogramy.Commands;
using Application.Harmonogramy.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class HarmonogramController : ApiControllerBase
    {
        //ustawia harmonogramy (według godzin pracy) weterynarzom na tydzień do przodu względem ostatniego harmonogramu w systemie
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [HttpPut("auto")]
        public async Task<IActionResult> AddWeterynarzHarmonograms(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateHarmonogramCommand
                {

                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //ustawia harmonogramy (według ich godzin pracy) tylko tym weterynarzom którzy danego dnia nie mają ustawionego harmonogramu
        //[Authorize(Roles = "admin")]
        /*[HttpPost("day")]
        public async Task<IActionResult> AddHarmonogramsForADay(DateTime date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateHarmonogramDefaultCommand
                {
                    Data = date
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/


        //ustawia harmonogramy weterynarzowi na podany dzień według godzin pracy
        //[Authorize(Roles = "admin")]
        /*[HttpPost("day/{ID_osoba}")]
        public async Task<IActionResult> AddWeterynarzHarmonogramForADay(DateTime date, string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateHarmonogramByIDCommand
                {
                    Data = date,
                    ID_weterynarz = ID_osoba
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/


        //usuwa harmonogramy na podany dzień wszystkim weterynarzom przy okazji anulując wizyty tego dnia
        //[Authorize(Roles = "admin")]
        /*[HttpDelete("day")]
        public async Task<IActionResult> DeleteHarmonogramsForADay(DateTime date, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteHarmonogramCommand
                {
                    Data = date
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/

        
        //[Authorize(Roles = "admin")]
        [HttpDelete("day/{ID_osoba}")]
        public async Task<IActionResult> DeleteWeterynarzHarmonogramForADay(DateTime date, string ID_osoba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteHarmonogramByIdCommand
                {
                    ID_osoba = ID_osoba,
                    Data = date
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //klient umawia wizytę albo pracownik kliniki umówia wizytę na prośbę klienta
        [Authorize(Roles = "klient,weterynarz,admin")]
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
        [Authorize(Roles = "admin")]
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


        [Authorize(Roles = "weterynarz,admin")]
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