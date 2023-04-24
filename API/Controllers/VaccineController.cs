using Application.DTO.Requests;
using Application.Szczepionki.Commands;
using Application.Szczepionki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class VaccineController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetSzczepionkaList(CancellationToken token, string search, int page)
        {
            try
            {
                return Ok(await Mediator.Send(new VaccineListQuery
                {
                    SearchWord = search,
                    Page = page
                }, token));
            } catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSzczepionka(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VaccineListAllQuery(), token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("details/{ID_szczepionka}")]
        public async Task<IActionResult> GetSzczepionkaDetails(string ID_szczepionka, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new VaccineDetailsQuery
                {
                    ID_szczepionka = ID_szczepionka
                }, token));
            } catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddSzczepionka(VaccineRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new CreateVaccineCommand
                {
                    request = request
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("{ID_szczepionka}")]
        public async Task<IActionResult> UpdateSzczepionka(string ID_szczepionka, VaccineRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateVaccineCommand
                {
                    ID_szczepionka = ID_szczepionka,
                    request = request
                }, token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_szczepionka}")]
        public async Task<IActionResult> DeleteSzczepionka(string ID_szczepionka, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteVaccineCommand
                {
                    ID_szczepionka = ID_szczepionka
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