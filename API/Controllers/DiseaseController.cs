using Application.Choroby.Commands;
using Application.Choroby.Queries;
using Application.DTO.Requests;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class DiseaseController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetChorobaList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new ChorobaListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllChoroba(CancellationToken token)
        {
            return Ok(await Mediator.Send(new ChorobaListAllQuery(), token));
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpGet("{ID_Choroba}")]
        public async Task<IActionResult> GetChorobaDetails(string ID_Choroba, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ChorobaDetailsQuery
                {
                    ID_Choroba = ID_Choroba
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddChoroba(DiseaseRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateDiseaseCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpPut("{ID_Choroba}")]
        public async Task<IActionResult> UpdateChoroba(string ID_Choroba, DiseaseRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateDiseaseCommand
                {
                    ID_Choroba = ID_Choroba,
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        [AuthorizeRoles(RoleEnum.Admin, RoleEnum.Weterynarz)]
        [HttpDelete("{ID_Choroba}")]
        public async Task<IActionResult> DeleteChoroba(string ID_Choroba, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteDiseaseCommand
                {
                    ID_Choroba = ID_Choroba
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