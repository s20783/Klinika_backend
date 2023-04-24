using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : Controller
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected string GetUserId()
        {
            var rolesClaims = this.User.Claims.Where(x => x.Type == "idUser").ToArray();
            if (rolesClaims.ToList().Any())
            {
                return rolesClaims[0].Value;
            }

            return "";
        }

        private string GetUserRole()
        {
            var rolesClaims = this.User.Claims.Where(x => x.Type == ClaimTypes.Role).ToArray();
            if (rolesClaims.ToList().Any())
            {
                return rolesClaims[0].Value;
            }

            return "";
        }

        protected bool isAdmin()
        {
            var role = GetUserRole();
            if (role.Equals(nameof(RoleEnum.Admin)))
            {
                return true;
            }

            return false;
        }

        protected bool isWeterynarz()
        {
            var role = GetUserRole();
            if (role.Equals(nameof(RoleEnum.Weterynarz)))
            {
                return true;
            }

            return false;
        }

        protected bool isKlient()
        {
            var role = GetUserRole();
            if (role.Equals(nameof(RoleEnum.Klient)))
            {
                return true;
            }

            return false;
        }
    }
}
