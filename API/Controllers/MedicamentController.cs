using Application.DTO.Requests;
using Application.Leki.Commands;
using Application.Leki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class MedicamentController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetLekList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new MedicamentListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("only")]
        public async Task<IActionResult> GetLekOnlyList(CancellationToken token)
        {
            return Ok(await Mediator.Send(new MedicamentOnlyListQuery
            {

            }, token));
        }

        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_lek}")]
        public async Task<IActionResult> GetLekById(string ID_lek, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new MedicamentDetailsQuery
                {
                    ID_lek = ID_lek
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddLek(MedicamentRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateMedicamentCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("{ID_lek}")]
        public async Task<IActionResult> UpdateLek(string ID_lek, MedicamentRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateMedicamentCommand
                {
                    ID_lek = ID_lek,
                    request = request
                }, token);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_lek}")]
        public async Task<IActionResult> DeleteLek(string ID_lek, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteMedicamentCommand
                {
                    ID_lek = ID_lek
                }, token);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}