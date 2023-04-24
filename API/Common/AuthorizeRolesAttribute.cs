using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PRO_API.Controllers;
using System;

namespace PRO_API.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params RoleEnum[] roles)
        {
            Roles = string.Join(",", Array.ConvertAll(roles, x => Enum.GetName(typeof(RoleEnum), x)));
        }
    }
}
