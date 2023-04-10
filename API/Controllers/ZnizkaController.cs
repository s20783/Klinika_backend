using Application.Znizki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class ZnizkaController : ApiControllerBase
    {
        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetZnizkaList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ZnizkaListQuery
                {

                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RolaEnum.Admin)]
        [HttpGet("{ID_znizka}")]
        public async Task<IActionResult> GetZnizkaDetails(string ID_znizka, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new ZnizkaDetailsQuery
                {
                    ID_znizka = ID_znizka
                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}