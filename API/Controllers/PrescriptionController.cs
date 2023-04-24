﻿using Application.DTO.Requests;
using Application.Recepty.Commands;
using Application.Recepty.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class PrescriptionController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_Klient}")]
        public async Task<IActionResult> GetReceptaKlientList(string ID_Klient, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PrescriptionClientQuery
                {
                    ID_klient = ID_Klient
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Klient)]
        [HttpGet("moje_recepty")]
        public async Task<IActionResult> GetReceptaKlientList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PrescriptionClientQuery
                {
                    ID_klient = GetUserId()
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet("details/{ID_Recepta}")]
        public async Task<IActionResult> GetReceptaDetails(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PrescriptionDetailsQuery
                {
                    ID_recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "klient,weterynarz,admin")]
        [HttpGet("pacjent/{ID_Pacjent}")]
        public async Task<IActionResult> GetReceptaPacjentList(string ID_Pacjent, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PrescriptionPatientQuery
                {
                    ID_pacjent = ID_Pacjent
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPost]
        public async Task<IActionResult> AddRecepta(string ID_Wizyta, string Zalecenia, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreatePrescriptionCommand
                {
                    ID_wizyta = ID_Wizyta,
                    Zalecenia = Zalecenia,
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpPut("{ID_Recepta}")]
        public async Task<IActionResult> UpdateRecepta(string ID_Recepta, string Zalecenia, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdatePrescriptionCommand
                {
                    ID_recepta = ID_Recepta,
                    Zalecenia = Zalecenia
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "weterynarz,admin")]
        [HttpDelete("{ID_Recepta}")]
        public async Task<IActionResult> DeleteRecepta(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DeletePrescriptionCommand
                {
                    ID_recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}