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
    public class SzczepionkaController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet]
        public async Task<IActionResult> GetSzczepionkaList(CancellationToken token, string search, int page)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepionkaListQuery
                {
                    SearchWord = search,
                    Page = page
                }, token));
            } catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpGet("details/{ID_szczepionka}")]
        public async Task<IActionResult> GetSzczepionkaDetails(string ID_szczepionka, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new SzczepionkaDetailsQuery
                {
                    ID_szczepionka = ID_szczepionka
                }, token));
            } catch (Exception)
            {
                return NotFound();
            }
        }


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPost]
        public async Task<IActionResult> AddSzczepionka(SzczepionkaRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new CreateSzczepionkaCommand
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


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpPut("{ID_szczepionka}")]
        public async Task<IActionResult> UpdateSzczepionka(string ID_szczepionka, SzczepionkaRequest request, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new UpdateSzczepionkaCommand
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


        [AuthorizeRoles(RolaEnum.Admin, RolaEnum.Weterynarz)]
        [HttpDelete("{ID_szczepionka}")]
        public async Task<IActionResult> DeleteSzczepionka(string ID_szczepionka, CancellationToken token)
        {
            try
            {
                await Mediator.Send(new DeleteSzczepionkaCommand
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