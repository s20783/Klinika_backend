using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PRO_API.Middlewares
{
    public class LogErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public LogErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request != null)
            {
                try
                {
                    await _next(httpContext);
                }
                catch (Exception e)
                {
                    using (StreamWriter fileStream = new StreamWriter(new FileStream(@"apiError.log", FileMode.Append)))
                    {
                        string error = e.ToString();
                        string requestPath = httpContext.Request.Path;
                        string queryString = httpContext.Request.Query.ToString();
                        string method = httpContext.Request.Method;
                        string outputString = requestPath + " " + queryString + " " + method + " error: " + error + " at: " + DateTime.Now + "\n";

                        await fileStream.WriteAsync(outputString);
                    }
                }
            }
        }
    }
}