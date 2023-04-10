using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRO_API.Middlewares
{
    public class LogActivityMiddleware
    {
        private readonly RequestDelegate _next;

        public LogActivityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await _next(httpContext);

            if (httpContext.Request.Method != "GET")
            {
                string userInfo = "";

                var rolesClaims = httpContext.User.Claims.Where(x => x.Type == "idUser" || x.Type == ClaimTypes.Role).ToArray();
                if (rolesClaims.ToList().Any())
                {
                    userInfo += "ID = " + rolesClaims[0].Value + ", Rola: " + rolesClaims[1].Value;
                }

                using (StreamWriter fileStream = new StreamWriter(new FileStream(@"api.log", FileMode.Append)))
                {
                    string requestPath = httpContext.Request.Path;
                    string query = httpContext.Request.Query.ToString();
                    string method = httpContext.Request.Method;
                    string body = httpContext.Request.Body.ToString();
                    string outputInfo = method + " " + requestPath + " " + query + " " + body + " at: " + DateTime.Now + " - (" + userInfo + ")" + "\n";

                    await fileStream.WriteAsync(outputInfo);
                }
            }
        }
    }
}