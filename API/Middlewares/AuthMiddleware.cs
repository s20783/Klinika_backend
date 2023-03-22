using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PRO_API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await _next(httpContext);
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await httpContext.Response.WriteAsync("Brak autoryzacji, odmowa dostępu.");
            }
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await httpContext.Response.WriteAsync("Nie masz dostępu do tej akcji.");
            }
        }
    }
}