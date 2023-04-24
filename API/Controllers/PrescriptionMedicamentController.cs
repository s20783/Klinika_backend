﻿using Application.ReceptaLeki.Commands;
using Application.ReceptaLeki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class PrescriptionMedicamentController : ApiControllerBase
    {
        [Authorize]
        [HttpGet("{ID_Recepta}")]
        public async Task<IActionResult> GetReceptaLek(string ID_Recepta, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new PrescriptionMedicamentLekQuery
                {
                    ID_Recepta = ID_Recepta
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddReceptaLek(string ID_Recepta, string ID_Lek, int Ilosc, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new AddPrescriptionMedicamentCommand
                {
                    ID_Recepta = ID_Recepta,
                    ID_Lek = ID_Lek,
                    Ilosc = Ilosc
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_Recepta}/{ID_Lek}")]
        public async Task<IActionResult> DeleteReceptaLek(string ID_Recepta, string ID_Lek, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DeletePrescriptionMedicamentCommand
                {
                    ID_Recepta = ID_Recepta,
                    ID_Lek = ID_Lek
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}