using Application.Znizki.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using PRO_API.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class DiscountController : ApiControllerBase
    {
        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetZnizkaList(CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DiscountListQuery
                {

                }, token));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [AuthorizeRoles(RoleEnum.Admin)]
        [HttpGet("{ID_znizka}")]
        public async Task<IActionResult> GetZnizkaDetails(string ID_znizka, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new DiscountDetailsQuery
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