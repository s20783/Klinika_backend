﻿using Application.Common.Exceptions;
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
    public class WizytaController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetWizytaList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new WizytaListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RolaEnum.Weterynarz, RolaEnum.Klient)]
        [HttpGet("moje_wizyty")]
        public async Task<IActionResult> GetWizytaKlient(CancellationToken token)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new WizytaKlientQuery
                    {
                        ID_klient = GetUserId()
                    }, token));
                }
                return Ok(await Mediator.Send(new WizytaWeterynarzQuery
                {
                    ID_weterynarz = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("{ID_klient}")]
        public async Task<IActionResult> GetWizytaKlient(string ID_klient, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new WizytaAdminQuery
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
                return Ok(await Mediator.Send(new WizytaPacjentQuery
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
                    return Ok(await Mediator.Send(new WizytaDetailsKlientQuery
                    {
                        ID_klient = GetUserId(),
                        ID_wizyta = ID_wizyta
                    }, token));
                }

                return Ok(await Mediator.Send(new WizytaDetailsAdminQuery
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
        public async Task<IActionResult> UmowWizyte(UmowWizyteRequest request, CancellationToken token)    //klient albo weterynarz lub admin umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new CreateWizytaKlientCommand
                    {
                        ID_klient = GetUserId(),
                        ID_pacjent = request.ID_Pacjent,
                        ID_Harmonogram = request.ID_Harmonogram,
                        Notatka = request.Notatka
                    }, token));
                }

                return Ok(await Mediator.Send(new CreateWizytaKlientCommand
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

        [AuthorizeRoles(RolaEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddWizyta(string ID_Harmonogram, string ID_Pacjent, CancellationToken token)    //klient albo weterynarz lub admin umówia wizytę dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateWizytaCommand
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
        public async Task<IActionResult> PrzelozWizyte(UmowWizyteRequest request, string ID_wizyta, CancellationToken token)   //klient albo weterynarz lub admin zmienia termin wizyty dla klienta (telefonicznie albo na miejscu)
        {
            try
            {
                if (isKlient())
                {
                    return Ok(await Mediator.Send(new UpdateWizytaDateCommand
                    {
                        ID_wizyta = ID_wizyta,
                        //ID_klient = GetUserId(),
                        ID_pacjent = request.ID_Pacjent,
                        ID_harmonogram = request.ID_Harmonogram,
                        Notatka = request.Notatka
                    }, token));
                }

                return Ok(await Mediator.Send(new UpdateWizytaDateCommand
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

        [AuthorizeRoles(RolaEnum.Weterynarz)]
        [HttpPut("{ID_wizyta}")]
        public async Task<IActionResult> UpdateWizytaInfo(WizytaInfoUpdateRequest request, string ID_wizyta, CancellationToken token)    //weterynarz zmienia informacje o wizycie (opis, status, dodaje wykonane usługi)
        {
            try
            {
                await Mediator.Send(new UpdateWizytaInfoCommand
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
                    return Ok(await Mediator.Send(new DeleteWizytaKlientCommand
                    {
                        ID_wizyta = ID_wizyta,
                        ID_klient = GetUserId()
                    }, token));
                }

                return Ok(await Mediator.Send(new DeleteWizytaKlientCommand
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

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpDelete("admin/{ID_wizyta}")]
        public async Task<IActionResult> DeleteWizytaByKlinika(string ID_wizyta, CancellationToken token)    //admin anuluje wizytę, status wizyty ustawiony jako anulowany przez klinike
        {
            try
            {
                await Mediator.Send(new DeleteWizytaAdminCommand
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