using Application.Common.Exceptions;
using Application.DTO.Requests;
using Application.Wizyty.Commands;
using Application.Wizyty.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class VisitController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetWizytaList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new VisitListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RoleEnum.Weterynarz, RoleEnum.Klient)]
        [HttpGet("moje_wizyty")]
        public async Task<IActionResult> GetWizytaKlient(CancellationToken token)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new VisitClientQuery
                    {
                        ID_klient = GetUserId()
                    }, token));
                }
                return Ok(await Mediator.Send(new VisitVetQuery
                {
                    ID_weterynarz = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_klient}")]
        public async Task<IActionResult> GetWizytaKlient(string ID_klient, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VisitAdminQuery
                {
                    ID_klient = ID_klient
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Authorize]
        [HttpGet("pacjent/{ID_Pacjent}")]
        public async Task<IActionResult> GetWizytaPacjent(string ID_Pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VisitPatientQuery
                {
                    ID_Pacjent = ID_Pacjent
                }, token));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        [Authorize]
        [HttpGet("details/{ID_wizyta}")]
        public async Task<IActionResult> GetWizytaDetails(string ID_wizyta, CancellationToken token)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new VisitDetailsClientQuery
                    {
                        ID_klient = GetUserId(),
                        ID_wizyta = ID_wizyta
                    }, token));
                }

                return Ok(await Mediator.Send(new VisitDetailsAdminQuery
                {
                    ID_wizyta = ID_wizyta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("umowWizyte")]
        public async Task<IActionResult> UmowWizyte(VisitRequest request, CancellationToken token)    //klient albo weterynarz lub admin umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new CreateVisitClientCommand
                    {
                        ID_klient = GetUserId(),
                        ID_pacjent = request.ID_Pacjent,
                        ID_Harmonogram = request.ID_Harmonogram,
                        Notatka = request.Notatka
                    }, token));
                }

                return Ok(await Mediator.Send(new CreateVisitClientCommand
                {
                    ID_klient = request.ID_Klient,
                    ID_pacjent = request.ID_Pacjent,
                    ID_Harmonogram = request.ID_Harmonogram,
                    Notatka = request.Notatka
                }, token));
            }
            catch (ConstraintException e)
            {
                return BadRequest(new
                {
                    message = e.Message,
                    value = e.ConstraintValue
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddWizyta(string ID_Harmonogram, string ID_Pacjent, CancellationToken token)    //klient albo weterynarz lub admin umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateVisitCommand
                {
                    ID_pacjent = ID_Pacjent,
                    ID_harmonogram = ID_Harmonogram
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("przeloz/{ID_wizyta}")]
        public async Task<IActionResult> PrzelozWizyte(VisitRequest request, string ID_wizyta, CancellationToken token)   //klient albo weterynarz lub admin zmienia termin wizyty dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new UpdateVisitDateCommand
                    {
                        ID_wizyta = ID_wizyta,
                        //ID_klient = GetUserId(),
                        ID_pacjent = request.ID_Pacjent,
                        ID_harmonogram = request.ID_Harmonogram,
                        Notatka = request.Notatka
                    }, token));
                }

                return Ok(await Mediator.Send(new UpdateVisitDateCommand
                {
                    ID_wizyta = ID_wizyta,
                    //ID_klient = request.ID_Klient,
                    ID_pacjent = request.ID_Pacjent,
                    ID_harmonogram = request.ID_Harmonogram,
                    Notatka = request.Notatka
                }, token));
            }
            /*catch (ConstraintException e)
            {
                return BadRequest(new
                {
                    message = e.Message,
                    value = e.ConstraintValue
                });
            }*/
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RoleEnum.Weterynarz)]
        [HttpPut("{ID_wizyta}")]
        public async Task<IActionResult> UpdateWizytaInfo(VisitInfoUpdateRequest request, string ID_wizyta, CancellationToken token)    //weterynarz zmienia informacje o wizycie (opis, status, dodaje wykonane usługi)
        {
            try
            {
                await Mediator.Send(new UpdateVisitInfoCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_weterynarz = GetUserId(),
                    request = request
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{ID_wizyta}")]
        public async Task<IActionResult> DeleteWizyta(string ID_wizyta, string ID_klient, CancellationToken token)  //klient albo weterynarz lub admin aunuluje wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new DeleteVisitClientCommand
                    {
                        ID_wizyta = ID_wizyta,
                        ID_klient = GetUserId()
                    }, token));
                }

                return Ok(await Mediator.Send(new DeleteVisitClientCommand
                {
                    ID_wizyta = ID_wizyta,
                    ID_klient = ID_klient
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("admin/{ID_wizyta}")]
        public async Task<IActionResult> DeleteWizytaByKlinika(string ID_wizyta, CancellationToken token)    //admin anuluje wizytę, status wizyty ustawiony jako anulowany przez klinike
        {
            try
            {
                await Mediator.Send(new DeleteVisitAdminCommand
                {
                    ID_wizyta = ID_wizyta
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