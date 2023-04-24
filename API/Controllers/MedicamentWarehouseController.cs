using Application.DTO.Request;
using Application.LekiWMagazynie.Commands;
using Application.LekiWMagazynie.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class MedicamentWarehouseController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_stan_leku}")]
        public async Task<IActionResult> GetLekWMagazynieById(string ID_stan_leku, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new MedicamentWarehouseQuery
                {
                    ID_stan_leku = ID_stan_leku
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost("{ID_lek}")]
        public async Task<IActionResult> AddStanLeku(string ID_lek, MedicamentWarehouseRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateMedicamentWarehouseCommand
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


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("{ID_stan_leku}")]
        public async Task<IActionResult> UpdateStanLeku(string ID_stan_leku, MedicamentWarehouseRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateMedicamentWarehouseCommand
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


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_stan_leku}")]
        public async Task<IActionResult> DeleteStanLeku(string ID_stan_leku, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteMedicamentWarehouseCommand
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