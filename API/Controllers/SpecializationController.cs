using Application.DTO.Request;
using Application.Specjalizacje.Commands;
using Application.Specjalizacje.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class SpecializationController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetSpecjalizacjaList(CancellationToken token, string search, int page)
        {
            return Ok(await Mediator.Send(new SpecializationListQuery
            {
                SearchWord = search,
                Page = page
            }, token));
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSpecjalizacja(CancellationToken token)
        {
            return Ok(await Mediator.Send(new SpecializationListAllQuery(), token));
        }


        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("details/{ID_specjalizacja}")]
        public async Task<IActionResult> GetSpecjalizacjaById(string ID_specjalizacja, CancellationToken token)
        {
            try 
            {
                return Ok(await Mediator.Send(new SpecializationDetailsQuery
                {
                    ID_specjalizacja = ID_specjalizacja
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddSpecjalizacja(SpecializationRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new CreateSpecializationCommand
                {
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpPut("{ID_specjalizacja}")]
        public async Task<IActionResult> UpdateSpecjalizacja(string ID_specjalizacja, SpecializationRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateSpecializationCommand
                {
                    ID_specjalizacja = ID_specjalizacja,
                    request = request
                }, token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpDelete("{ID_specjalizacja}")]
        public async Task<IActionResult> DeleteSpecjalizacja(string ID_specjalizacja, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteSpecializationCommand
                {
                    ID_specjalizacja = ID_specjalizacja
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