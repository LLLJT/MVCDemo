using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MVCDemo.Controllers
{
    public class RequestSetOptionsMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestSetOptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //
        public async Task Invoke(HttpContext httpContext)
        {
            var option = httpContext.Request.Query["option"];

            if (!string.IsNullOrWhiteSpace(option))
            {
                httpContext.Items["option"] = WebUtility.HtmlEncode(option);
            }

            await _next(httpContext);

        }

    }
}
